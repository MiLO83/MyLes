// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;

namespace MyNes.Core
{
    /*APU section*/
    public partial class NesEmu
    {
        // DATA REG
        private static byte apu_reg_io_db;// The data bus
        private static byte apu_reg_io_addr;// The address bus
        private static bool apu_reg_access_happened;// Triggers when cpu accesses apu bus.
        private static bool apu_reg_access_w;// True= write access, False= Read access.
        private static Action[] apu_reg_update_func;
        private static Action[] apu_reg_read_func;
        private static Action[] apu_reg_write_func;
        private static Action apu_update_playback;

        private static bool apu_odd_cycle;
        private static bool apu_irq_enabled;
        private static bool apu_irq_flag;
        private static bool apu_irq_do_it;
        internal static bool apu_irq_delta_occur;

        private static bool apu_seq_mode;
        private static int apu_ferq_f;// IRQ clock frequency
        private static int apu_ferq_l;// Length counter clock
        private static int apu_ferq_e;// Envelope counter clock
        private static int apu_cycle_f;
        private static int apu_cycle_f_t;
        private static int apu_cycle_e;
        private static int apu_cycle_l;
        private static bool apu_odd_l;
        private static bool apu_check_irq;
        private static bool apu_do_env;
        private static bool apu_do_length;
        /*Playback*/
        public static bool SoundEnabled;
        private static bool audio_filters_enabled;

        public static double cpu_clock_per_frame;
        internal static double apu_target_samples_count_per_frame;
        private static byte[][] audio_samples;
        private static int audio_w_pos;
        private static int audio_samples_added;
        internal static int audio_samples_count;
        private static double[] tnd_table;
        private static double[] pulse_table;
        private static double[] mixer_small_table;
        private static double[] mixer_big_table;
        private static double[] channels_out;

        // Output values
        private static double audio_x;
        private static byte audio_y;
        private static double audio_bp_ratio1;
        private static double audio_bp_ratio1_timer;

        private static double audio_x_av;
        private static double audio_x_clk;

        private static double audio_right_x;
        private static byte audio_right_y;
        private static double audio_right_x_av;
        private static double audio_right_x_clk;

        private static double audio_left_x;
        private static byte audio_left_y;
        private static double audio_left_x_av;
        private static double audio_left_x_clk;

        private static double[] audio_left_channels_vol;
        private static double[] audio_right_channels_vol;

        private static SoundLowPassFilter audio_low_pass_filter_14K;
        private static SoundHighPassFilter audio_high_pass_filter_90;
        private static SoundHighPassFilter audio_high_pass_filter_440;

        private static bool audio_sq1_outputable;
        private static bool audio_sq2_outputable;
        private static bool audio_nos_outputable;
        private static bool audio_trl_outputable;
        private static bool audio_dmc_outputable;

        // External sound channels activation
        private static bool apu_use_external_sound;

        private static void APUInitialize()
        {
            apu_reg_update_func = new Action[0x20];
            apu_reg_read_func = new Action[0x20];
            apu_reg_write_func = new Action[0x20];
            for (int i = 0; i < 0x20; i++)
            {
                apu_reg_update_func[i] = APUBlankAccess;
                apu_reg_read_func[i] = APUBlankAccess;
                apu_reg_write_func[i] = APUBlankAccess;
            }

            apu_reg_update_func[0x00] = APUOnRegister4000;
            apu_reg_update_func[0x01] = APUOnRegister4001;
            apu_reg_update_func[0x02] = APUOnRegister4002;
            apu_reg_update_func[0x03] = APUOnRegister4003;
            apu_reg_update_func[0x04] = APUOnRegister4004;
            apu_reg_update_func[0x05] = APUOnRegister4005;
            apu_reg_update_func[0x06] = APUOnRegister4006;
            apu_reg_update_func[0x07] = APUOnRegister4007;
            apu_reg_update_func[0x08] = APUOnRegister4008;
            apu_reg_update_func[0x09] = APUOnRegister4009;
            apu_reg_update_func[0x0A] = APUOnRegister400A;
            apu_reg_update_func[0x0B] = APUOnRegister400B;
            apu_reg_update_func[0x0C] = APUOnRegister400C;
            apu_reg_update_func[0x0D] = APUOnRegister400D;
            apu_reg_update_func[0x0E] = APUOnRegister400E;
            apu_reg_update_func[0x0F] = APUOnRegister400F;
            apu_reg_update_func[0x10] = APUOnRegister4010;
            apu_reg_update_func[0x11] = APUOnRegister4011;
            apu_reg_update_func[0x12] = APUOnRegister4012;
            apu_reg_update_func[0x13] = APUOnRegister4013;
            //apu_reg_update_func[0x14] = APUOnRegister4014;
            apu_reg_update_func[0x15] = APUOnRegister4015;
            apu_reg_update_func[0x16] = APUOnRegister4016;
            apu_reg_update_func[0x17] = APUOnRegister4017;

            apu_reg_read_func[0x15] = APURead4015;
            apu_reg_read_func[0x16] = APURead4016;
            apu_reg_read_func[0x17] = APURead4017;

            apu_reg_write_func[0x14] = APUWrite4014;
            apu_reg_write_func[0x15] = APUWrite4015;

            audio_low_pass_filter_14K = new SoundLowPassFilter(0.00815686);// 14KHz
            audio_high_pass_filter_90 = new SoundHighPassFilter(0.999835);// 90 Hz
            audio_high_pass_filter_440 = new SoundHighPassFilter(0.996039);// 440 Hz

            channels_out = new double[5];
            tnd_table = new double[204];
            pulse_table = new double[32];
            mixer_small_table = new double[16];
            audio_left_channels_vol = new double[5];
            audio_right_channels_vol = new double[5];
            mixer_big_table = new double[128];

            for (int n = 0; n < 32; n++)
                pulse_table[n] = 95.52 / (8128.0 / n + 100);

            for (int n = 0; n < 204; n++)
                tnd_table[n] = 163.67 / (24329.0 / n + 100);


            for (int n = 0; n < 16; n++)
                GetPrec(n, 0xF, 0.14, out mixer_small_table[n]);// Normalize all channels except dmc output from 0-15 into 0 - 14 % of the mix

            for (int n = 0; n < 128; n++)
                GetPrec(n, 0x7F, 0.44, out mixer_big_table[n]);// Normalize DMC-PCM output from 0-127 into 0 - 44 % of the mix

            // Mono nes by default
            apu_update_playback = APUUpdatePlayback;

            ApplyAudioPANSettings();
        }
        public static void ApplyAudioPANSettings()
        {
            // Adjust pan for stereo nes
            double channel_sq1 = ((double)MyNesMain.RendererSettings.Audio_PAN_Sq1 / 100);

            if (channel_sq1 > 1.0)
            {
                // Adjusting right channel
                audio_left_channels_vol[0] = 1.0;
                audio_right_channels_vol[0] = 2.0 - channel_sq1;
            }
            else if (channel_sq1 == 1.0)
            {
                audio_right_channels_vol[0] = 1.0;
                audio_left_channels_vol[0] = 1.0;
            }
            else
            {
                audio_right_channels_vol[0] = 1.0;
                audio_left_channels_vol[0] = channel_sq1;
            }

            double channel_sq2 = ((double)MyNesMain.RendererSettings.Audio_PAN_Sq2 / 100);

            if (channel_sq2 > 1.0)
            {
                // Adjusting right channel
                audio_left_channels_vol[1] = 1.0;
                audio_right_channels_vol[1] = 2.0 - channel_sq2;
            }
            else if (channel_sq2 == 1.0)
            {
                audio_right_channels_vol[1] = 1.0;
                audio_left_channels_vol[1] = 1.0;
            }
            else
            {
                audio_right_channels_vol[1] = 1.0;
                audio_left_channels_vol[1] = channel_sq2;
            }

            double channel_nos = ((double)MyNesMain.RendererSettings.Audio_PAN_Nos / 100);

            if (channel_nos > 1.0)
            {
                // Adjusting right channel
                audio_left_channels_vol[2] = 1.0;
                audio_right_channels_vol[2] = 2.0 - channel_nos;
            }
            else if (channel_nos == 1.0)
            {
                audio_right_channels_vol[2] = 1.0;
                audio_left_channels_vol[2] = 1.0;
            }
            else
            {
                audio_right_channels_vol[2] = 1.0;
                audio_left_channels_vol[2] = channel_nos;
            }

            double channel_trl = ((double)MyNesMain.RendererSettings.Audio_PAN_Trl / 100);

            if (channel_trl > 1.0)
            {
                // Adjusting right channel
                audio_left_channels_vol[3] = 1.0;
                audio_right_channels_vol[3] = 2.0 - channel_trl;
            }
            else if (channel_trl == 1.0)
            {
                audio_right_channels_vol[3] = 1.0;
                audio_left_channels_vol[3] = 1.0;
            }
            else
            {
                audio_right_channels_vol[3] = 1.0;
                audio_left_channels_vol[3] = channel_trl;
            }

            double channel_dmc = ((double)MyNesMain.RendererSettings.Audio_PAN_DMC / 100);

            if (channel_dmc > 1.0)
            {
                // Adjusting right channel
                audio_left_channels_vol[4] = 1.0;
                audio_right_channels_vol[4] = 2.0 - channel_dmc;
            }
            else if (channel_dmc == 1.0)
            {
                audio_right_channels_vol[4] = 1.0;
                audio_left_channels_vol[4] = 1.0;
            }
            else
            {
                audio_right_channels_vol[4] = 1.0;
                audio_left_channels_vol[4] = channel_dmc;
            }
        }
        public static void ApplyAudioSettings(bool all = true)
        {
            SoundEnabled = MyNesMain.RendererSettings.Audio_SoundEnabled;
            audio_filters_enabled = MyNesMain.RendererSettings.Audio_FiltersEnabled;
            // Channel outputs
            audio_sq1_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ1;
            audio_sq2_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ2;
            audio_nos_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_NOZ;
            audio_trl_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_TRL;
            audio_dmc_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_DMC;

            if (apu_use_external_sound)
                mem_board.APUApplyChannelsSettings();
            if (all)
                CalculateAudioPlaybackValues();
            if (MyNesMain.RendererSettings.Audio_Channels == 2)
            {
                if (MyNesMain.RendererSettings.Audio_StereoNes)
                    apu_update_playback = APUUpdatePlayback_StereoNes;
                else
                    apu_update_playback = APUUpdatePlayback;
            }
            else
            {
                apu_update_playback = APUUpdatePlayback;
            }
            ApplyAudioPANSettings();
        }
        private static void APUHardReset()
        {
            apu_reg_io_db = 0;
            apu_reg_io_addr = 0;
            apu_reg_access_happened = false;
            apu_reg_access_w = false;
            apu_seq_mode = false;
            apu_odd_cycle = true;
            apu_cycle_f_t = 0;
            apu_cycle_e = 4;
            apu_cycle_f = 4;
            apu_cycle_l = 4;
            apu_odd_l = false;
            apu_check_irq = false;
            apu_do_env = false;
            apu_do_length = false;
            switch (Region)
            {
                case EmuRegion.NTSC:
                    {
                        cpu_clock_per_frame = 29780.5;// 29780.5 ?
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
                // TODO: define APU clock frequencies at PAL/DENDY
                case EmuRegion.PALB:
                    {
                        cpu_clock_per_frame = 33247.5;// 33247.5 ?
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
                case EmuRegion.DENDY:
                    {
                        cpu_clock_per_frame = 35464;
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
            }

            SQ1HardReset();
            SQ2HardReset();
            NOSHardReset();
            DMCHardReset();
            TRLHardReset();

            apu_irq_enabled = true;
            apu_irq_flag = false;
            reg_2004 = 0x2004;
            CalculateAudioPlaybackValues();
            // External sound channel activation
            apu_use_external_sound = mem_board.enable_external_sound;
            if (apu_use_external_sound)
                Tracer.WriteInformation("External sound channels has been enabled on apu.");

            if (MyNesMain.RendererSettings.Audio_Channels == 2)
            {
                if (MyNesMain.RendererSettings.Audio_StereoNes)
                    apu_update_playback = APUUpdatePlayback_StereoNes;
                else
                    apu_update_playback = APUUpdatePlayback;
            }
            else
            {
                apu_update_playback = APUUpdatePlayback;
            }
            ApplyAudioPANSettings();
        }
        private static void APUSoftReset()
        {
            apu_reg_io_db = 0;
            apu_reg_io_addr = 0;
            apu_reg_access_happened = false;
            apu_reg_access_w = false;
            apu_seq_mode = false;
            apu_odd_cycle = false;
            apu_cycle_f_t = 0;
            apu_cycle_e = 4;
            apu_cycle_f = 4;
            apu_cycle_l = 4;
            apu_odd_l = false;
            apu_check_irq = false;
            apu_do_env = false;
            apu_do_length = false;
            apu_irq_enabled = true;
            apu_irq_flag = false;
            SQ1SoftReset();
            SQ2SoftReset();
            TRLSoftReset();
            NOSSoftReset();
            DMCSoftReset();
        }
        private static void APUIORead(ref ushort addr, out byte value)
        {
            if (addr >= 0x4020)
                mem_board.ReadEX(ref addr, out value);
            else
            {
                apu_reg_io_addr = (byte)(addr & 0x1F);
                apu_reg_access_happened = true;
                apu_reg_access_w = false;
                apu_reg_read_func[apu_reg_io_addr]();
                value = apu_reg_io_db;
            }
        }
        private static void APUIOWrite(ref ushort addr, ref byte value)
        {
            if (addr >= 0x4020)
                mem_board.WriteEX(ref addr, ref value);
            else
            {
                apu_reg_io_addr = (byte)(addr & 0x1F);
                apu_reg_io_db = value;
                apu_reg_access_w = true;
                apu_reg_access_happened = true;
                apu_reg_write_func[apu_reg_io_addr]();
            }
        }

        #region IO Registers
        private static void APUBlankAccess()
        {
        }
        private static void APUWrite4014()
        {
            // OAM DMA
            dma_Oamaddress = (ushort)(apu_reg_io_db << 8);
            AssertOAMDMA();
        }
        private static void APUWrite4015()
        {
            // DMC DMA
            if ((apu_reg_io_db & 0x10) != 0)
            {
                if (dmc_dmaSize == 0)
                {
                    dmc_dmaSize = dmc_size_refresh;
                    dmc_dmaAddr = dmc_addr_refresh;
                }
            }
            else { dmc_dmaSize = 0; }

            if (!dmc_bufferFull && dmc_dmaSize > 0)
            {
                AssertDMCDMA();
            }
        }
        private static void APUOnRegister4015()
        {
            if (apu_reg_access_w)
            {
                // do a normal write
                SQ1On4015();
                SQ2On4015();
                NOSOn4015();
                TRLOn4015();
                DMCOn4015();
            }
            else
            {
                // on reads, do the effects we know
                apu_irq_flag = false;
                IRQFlags &= ~IRQ_APU;
            }
        }
        private static void APUOnRegister4016()
        {
            // Only writes accepted
            if (apu_reg_access_w)
            {
                if (inputStrobe > (apu_reg_io_db & 0x01))
                {
                    if (IsFourPlayers)
                    {
                        PORT0 = joypad3.GetData() << 8 | joypad1.GetData() | 0x01010000;
                        PORT1 = joypad4.GetData() << 8 | joypad2.GetData() | 0x02020000;
                    }
                    else
                    {
                        PORT0 = joypad1.GetData() | 0x01010100;
                        PORT1 = joypad2.GetData() | 0x02020200;
                    }
                }
                inputStrobe = apu_reg_io_db & 0x01;
            }
        }
        private static void APUOnRegister4017()
        {
            if (apu_reg_access_w)
            {
                apu_seq_mode = (apu_reg_io_db & 0x80) != 0;
                apu_irq_enabled = (apu_reg_io_db & 0x40) == 0;

                // Reset counters
                apu_cycle_e = -1;
                apu_cycle_l = -1;
                apu_cycle_f = -1;
                apu_odd_l = false;
                // Clock immediately ?
                apu_do_length = apu_seq_mode;
                apu_do_env = apu_seq_mode;
                // Reset irq
                apu_check_irq = false;

                if (!apu_irq_enabled)
                {
                    apu_irq_flag = false;
                    IRQFlags &= ~IRQ_APU;
                }
            }
        }

        private static void APURead4015()
        {
            apu_reg_io_db = (byte)(apu_reg_io_db & 0x20);
            // Channels enable
            SQ1Read4015();
            SQ2Read4015();
            NOSRead4015();
            TRLRead4015();
            DMCRead4015();
            // IRQ
            if (apu_irq_flag)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xBF) | 0x40);
            if (apu_irq_delta_occur)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0x7F) | 0x80);

        }
        private static void APURead4016()
        {
            apu_reg_io_db = (byte)(PORT0 & 1);
            PORT0 >>= 1;
        }
        private static void APURead4017()
        {
            apu_reg_io_db = (byte)(PORT1 & 1);
            PORT1 >>= 1;
        }
        #endregion

        private static void APUClock()
        {
            apu_odd_cycle = !apu_odd_cycle;

            if (apu_do_env)
                APUClockEnvelope();

            if (apu_do_length)
                APUClockDuration();

            if (apu_odd_cycle)
            {
                // IRQ
                apu_cycle_f++;
                if (apu_cycle_f >= apu_ferq_f)
                {
                    apu_cycle_f = -1;
                    apu_check_irq = true;
                    apu_cycle_f_t = 3;
                }
                // Envelope
                apu_cycle_e++;
                if (apu_cycle_e >= apu_ferq_e)
                {
                    apu_cycle_e = -1;
                    // Clock envelope and other units except when:
                    // 1 the seq mode is set
                    // 2 it is the time of irq check clock
                    if (apu_check_irq)
                    {
                        if (!apu_seq_mode)
                        {
                            // this is the 3rd step of mode 0, do a reset
                            apu_do_env = true;
                        }
                        else
                        {
                            // the next step will be the 4th step of mode 1
                            // so, shorten the step then do a reset
                            apu_cycle_e = 4;
                        }
                    }
                    else
                        apu_do_env = true;
                }
                // Length
                apu_cycle_l++;
                if (apu_cycle_l >= apu_ferq_l)
                {
                    apu_odd_l = !apu_odd_l;

                    apu_cycle_l = apu_odd_l ? -2 : -1;

                    // Clock duration and sweep except when:
                    // 1 the seq mode is set
                    // 2 it is the time of irq check clock
                    if (apu_check_irq && apu_seq_mode)
                    {
                        apu_cycle_l = 3730;// Next step will be after 7456 - 3730 = 3726 cycles, 2 cycles shorter than e freq
                        apu_odd_l = true;
                    }
                    else
                    {
                        apu_do_length = true;
                    }
                }

                SQ1Clock();
                SQ2Clock();
                NOSClock();

                if (apu_use_external_sound)
                    mem_board.OnAPUClock();

                if (apu_reg_access_happened)
                {
                    apu_reg_access_happened = false;
                    apu_reg_update_func[apu_reg_io_addr]();
                }

            }

            // Clock internal components
            TRLClock();
            DMCClock();

            if (apu_check_irq)
            {
                if (!apu_seq_mode)
                    APUCheckIRQ();
                // This is stupid ... :(
                apu_cycle_f_t--;
                if (apu_cycle_f_t == 0)
                    apu_check_irq = false;
            }
            if (apu_use_external_sound)
                mem_board.OnAPUClockSingle();

            apu_update_playback();
        }
        private static void APUClockDuration()
        {
            SQ1ClockLength();
            SQ2ClockLength();
            NOSClockLength();
            TRLClockLength();
            if (apu_use_external_sound)
                mem_board.OnAPUClockDuration();
            apu_do_length = false;
        }
        private static void APUClockEnvelope()
        {
            SQ1ClockEnvelope();
            SQ2ClockEnvelope();
            NOSClockEnvelope();
            TRLClockEnvelope();
            if (apu_use_external_sound)
                mem_board.OnAPUClockEnvelope();
            apu_do_env = false;
        }
        private static void APUCheckIRQ()
        {
            if (apu_irq_enabled)
                apu_irq_flag = true;
            if (apu_irq_flag)
                IRQFlags |= IRQ_APU;
        }

        public static void CalculateAudioPlayback(double low_frq)
        {
            // Calculate ratio 1
            double r1 = low_frq / emu_time_target_fps;
            // Ratio = cpu cycles per frames / target samples each frame
            audio_bp_ratio1 = cpu_clock_per_frame / r1;
        }
        private static void CalculateAudioPlaybackValues()
        {
            // Playback buffer
            // Calculate how many samples should be rendered each frame

            // Calculate ratio 2
            double r2 = (double)MyNesMain.RendererSettings.Audio_Frequency / emu_time_target_fps;
            // Ratio = cpu cycles per frames / target samples each frame
            audio_bp_ratio1 = cpu_clock_per_frame / r2;

            audio_bp_ratio1_timer = audio_bp_ratio1_timer = 0;

            audio_samples_count = (int)(r2 + 100);// target samples per frame + 100

            audio_samples = new byte[audio_samples_count][];
            for (int i = 0; i < audio_samples_count; i++)
                audio_samples[i] = new byte[2];

            audio_w_pos = 0;
            audio_samples_added = 0;
            audio_x = audio_y = 0;
            Tracer.WriteLine("AUDIO: frequency = " + MyNesMain.RendererSettings.Audio_Frequency);
            Tracer.WriteLine("AUDIO: timer ratio 1 = " + audio_bp_ratio1);
            Tracer.WriteLine("AUDIO: internal samples count = " + audio_samples_count);
        }
        private static void APUUpdatePlayback()
        {
            if (!SoundEnabled)
                return;

            audio_bp_ratio1_timer++;

            // Clock on playback frequency (i.e. 44100 Hz), on ratio of (how many pc clocks each frame / how many samples of playback each frame)
            // Here is clock on cpu timing, 1000 millisecond / ~ 179 MHz
            if (audio_bp_ratio1_timer >= audio_bp_ratio1)
            {
                audio_bp_ratio1_timer -= audio_bp_ratio1;

                if (MyNesMain.RendererSettings.Audio_UseLookupTable)
                {
                    // use look-up table as described here: <https://wiki.nesdev.com/w/index.php?title=APU_Mixer>
                    audio_x = pulse_table[sq1_output + sq2_output] + tnd_table[(3 * trl_output) + (2 * nos_output) + dmc_output];
                }
                else
                {
                    audio_x = (mixer_small_table[sq1_output] + mixer_small_table[sq2_output] + mixer_small_table[trl_output] + mixer_small_table[nos_output] + mixer_big_table[dmc_output]);// 0 - 1.0 ( 0 - 100 %)
                }

                if (apu_use_external_sound)
                {
                    audio_x = (audio_x + mem_board.APUGetSample()) / 2; // should be 0-1.0 (0 - 100 %) since expected mixed value from external sound channels should be 0 to 1.0 as well
                }

                audio_x_av = audio_x * 127;// There is no possible other transform from 0-1 into bits. It works only from range 0-1 into 0-127.
                audio_x_av++;// Add 1 to make sure there is no sample 0, db fix. See <https://github.com/jegqamas/Docs/blob/main/Audio%20And%20DB.txt>

                audio_y = (byte)((int)audio_x_av & 0xFF);

                // Write into buffer
                if (audio_w_pos < audio_samples_count)
                {
                    audio_samples[audio_w_pos][0] = audio_y;
                    audio_samples[audio_w_pos][1] = audio_y;

                    if (MyNesMain.WaveRecorder.IsRecording)
                        MyNesMain.WaveRecorder.AddSample(audio_y, audio_y);

                    audio_w_pos++;
                    audio_samples_added++;
                }
            }
        }
        private static void APUUpdatePlayback_StereoNes()
        {
            if (!SoundEnabled)
                return;

            // Audio playback with stereo nes mode.
            audio_bp_ratio1_timer++;

            // Clock on playback frequency (i.e. 44100 Hz)
            if (audio_bp_ratio1_timer >= audio_bp_ratio1)
            {
                audio_bp_ratio1_timer -= audio_bp_ratio1;

                /*LEFT CHANNEL*/
                if (MyNesMain.RendererSettings.Audio_UseLookupTable)
                {
                    // use look-up table as described here: <https://wiki.nesdev.com/w/index.php?title=APU_Mixer>
                    audio_left_x = pulse_table[(int)(sq1_output * audio_left_channels_vol[0]) + (int)(sq2_output * audio_left_channels_vol[1])] + tnd_table[(3 * (int)(trl_output * audio_left_channels_vol[3])) + (2 * (int)(nos_output * audio_left_channels_vol[2])) + (int)(dmc_output * audio_left_channels_vol[4])];
                }
                else
                {
                    // Use built-in mixer (My Nes Mixer).
                    audio_left_x = (mixer_small_table[(int)(sq1_output * audio_left_channels_vol[0])] + mixer_small_table[(int)(sq2_output * audio_left_channels_vol[1])] + mixer_small_table[(int)(trl_output * audio_left_channels_vol[3])] + mixer_small_table[(int)(nos_output * audio_left_channels_vol[2])] + mixer_big_table[(int)(dmc_output * audio_left_channels_vol[4])]);// 0 - 1.0 ( 0 - 100 %)

                }

                if (apu_use_external_sound)
                {
                    audio_left_x = (audio_left_x + mem_board.APUGetSample()) / 2; // should be 0-1.0 (0 - 100 %) since expected mixed value from external sound channels should be 0 to 1.0 as well
                }

                // There is no possible other transform from 0-1 into bits. It works only from range 0-1 into 0-127.
                // Add 1 to make sure there is no sample 0, db fix. See <https://github.com/jegqamas/Docs/blob/main/Audio%20And%20DB.txt>
                audio_left_x_av = (audio_left_x * 127) + 1;

                /*RIGHT CHANNEL*/
                if (MyNesMain.RendererSettings.Audio_UseLookupTable)
                {
                    // use look-up table as described here: <https://wiki.nesdev.com/w/index.php?title=APU_Mixer>
                    audio_right_x = pulse_table[(int)(sq1_output * audio_right_channels_vol[0]) + (int)(sq2_output * audio_right_channels_vol[1])] + tnd_table[(3 * (int)(trl_output * audio_right_channels_vol[3])) + (2 * (int)(nos_output * audio_right_channels_vol[2])) + (int)(dmc_output * audio_right_channels_vol[4])];
                }
                else
                {
                    // Use built-in mixer (My Nes Mixer).
                    audio_right_x = (mixer_small_table[(int)(sq1_output * audio_right_channels_vol[0])] + mixer_small_table[(int)(sq2_output * audio_right_channels_vol[1])] + mixer_small_table[(int)(trl_output * audio_right_channels_vol[3])] + mixer_small_table[(int)(nos_output * audio_right_channels_vol[2])] + mixer_big_table[(int)(dmc_output * audio_right_channels_vol[4])]);// 0 - 1.0 ( 0 - 100 %)

                }

                if (apu_use_external_sound)
                {
                    audio_right_x = (audio_right_x + mem_board.APUGetSample()) / 2; // should be 0-1.0 (0 - 100 %) since expected mixed value from external sound channels should be 0 to 1.0 as well
                }
                // There is no possible other transform from 0-1 into bits. It works only from range 0-1 into 0-127.
                // Add 1 to make sure there is no sample 0, db fix. See <https://github.com/jegqamas/Docs/blob/main/Audio%20And%20DB.txt>
                audio_right_x_av = (audio_right_x * 127) + 1;

                // Write into buffer
                if (audio_w_pos < audio_samples_count)
                {
                    audio_right_y = (byte)((int)audio_right_x_av & 0xFF);
                    audio_left_y = (byte)((int)audio_left_x_av & 0xFF);

                    // Sample is constructed like this: left channel | right channel
                    audio_samples[audio_w_pos][0] = audio_right_y;
                    audio_samples[audio_w_pos][1] = audio_left_y;

                    if (MyNesMain.WaveRecorder.IsRecording)
                        MyNesMain.WaveRecorder.AddSample(audio_right_y, audio_left_y);

                    audio_w_pos++;
                    audio_samples_added++;
                }
            }
        }

        private static void GetPrec(int inVal, int inMax, int outMax, out int val)
        {
            val = (outMax * inVal) / inMax;
        }
        private static void GetPrec(double inVal, double inMax, double outMax, out double val)
        {
            val = (outMax * inVal) / inMax;
        }
        private static void APUWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(apu_reg_io_db);
            bin.Write(apu_reg_io_addr);
            bin.Write(apu_reg_access_happened);
            bin.Write(apu_reg_access_w);
            bin.Write(apu_odd_cycle);
            bin.Write(apu_irq_enabled);
            bin.Write(apu_irq_flag);
            bin.Write(apu_irq_delta_occur);

            bin.Write(apu_seq_mode);
            bin.Write(apu_ferq_f);
            bin.Write(apu_ferq_l);
            bin.Write(apu_ferq_e);
            bin.Write(apu_cycle_f);
            bin.Write(apu_cycle_e);
            bin.Write(apu_cycle_l);
            bin.Write(apu_odd_l);
            bin.Write(apu_cycle_f_t);

            bin.Write(apu_check_irq);
            bin.Write(apu_do_env);
            bin.Write(apu_do_length);

            SQ1WriteState(ref bin);
            SQ2WriteState(ref bin);
            NOSWriteState(ref bin);
            TRLWriteState(ref bin);
            DMCWriteState(ref bin);
        }
        private static void APUReadState(ref System.IO.BinaryReader bin)
        {
            apu_reg_io_db = bin.ReadByte();
            apu_reg_io_addr = bin.ReadByte();
            apu_reg_access_happened = bin.ReadBoolean();
            apu_reg_access_w = bin.ReadBoolean();
            apu_odd_cycle = bin.ReadBoolean();
            apu_irq_enabled = bin.ReadBoolean();
            apu_irq_flag = bin.ReadBoolean();
            apu_irq_delta_occur = bin.ReadBoolean();

            apu_seq_mode = bin.ReadBoolean();
            apu_ferq_f = bin.ReadInt32();
            apu_ferq_l = bin.ReadInt32();
            apu_ferq_e = bin.ReadInt32();
            apu_cycle_f = bin.ReadInt32();
            apu_cycle_e = bin.ReadInt32();
            apu_cycle_l = bin.ReadInt32();
            apu_odd_l = bin.ReadBoolean();
            apu_cycle_f_t = bin.ReadInt32();

            apu_check_irq = bin.ReadBoolean();
            apu_do_env = bin.ReadBoolean();
            apu_do_length = bin.ReadBoolean();

            SQ1ReadState(ref bin);
            SQ2ReadState(ref bin);
            NOSReadState(ref bin);
            TRLReadState(ref bin);
            DMCReadState(ref bin);
        }
    }
}
