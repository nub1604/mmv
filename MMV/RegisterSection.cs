using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace memoryMapViewer
{
    public class RegisterSection
    {
        public string Label { get; set; } = "";
        public string LabelShort { get; set; } = "";
        public string Style { get; set; } = "";
        public string Access { get; set; } = "";
        public string Timing { get; set; } = "";
        [JsonConverter(typeof(HexIntConverter<int>))]
        public int Address { get; set; }
    }

   

    public  class RegistersList
    {
       



        public static readonly List<RegisterSection> CPU_Registers = new()
    {
        new RegisterSection { Label = "Interrupt Enable Register", Address = 0x4200, LabelShort = "NMITIMEN", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "IO Port Write Register", Address = 0x4201, LabelShort = "WRIO", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Multiplicand Registers", Address = 0x4202, LabelShort = "WRMPYA", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Multiplicand Registers", Address = 0x4203, LabelShort = "WRMPYB", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Divisor & Dividend Registers", Address = 0x4204, LabelShort = "WRDIVL", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Divisor & Dividend Registers", Address = 0x4205, LabelShort = "WRDIVH", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Divisor & Dividend Registers", Address = 0x4206, LabelShort = "WRDIVB", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "IRQ Timer Registers (Horizontal - Low)", Address = 0x4207, LabelShort = "HTIMEL", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "IRQ Timer Registers (Horizontal - High)", Address = 0x4208, LabelShort = "HTIMEH", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "IRQ Timer Registers (Vertical - Low)", Address = 0x4209, LabelShort = "VTIMEL", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "IRQ Timer Registers (Vertical - High)", Address = 0x420A, LabelShort = "VTIMEH", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "DMA Enable Register", Address = 0x420B, LabelShort = "MDMAEN", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "HDMA Enable Register", Address = 0x420C, LabelShort = "HDMAEN", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "ROM Speed Register", Address = 0x420D, LabelShort = "MEMSEL", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Interrupt Flag Registers", Address = 0x4210, LabelShort = "RDNMI", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Interrupt Flag Registers", Address = 0x4211, LabelShort = "TIMEUP", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "PPU Status Register", Address = 0x4212, LabelShort = "HVBJOY", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "IO Port Read Register", Address = 0x4213, LabelShort = "RDIO", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Multiplication Or Divide Result Registers (Low)", Address = 0x4214, LabelShort = "RDDIVL", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Multiplication Or Divide Result Registers (High)", Address = 0x4215, LabelShort = "RDDIVH", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Multiplication Or Divide Result Registers (Low)", Address = 0x4216, LabelShort = "RDMPYL", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Multiplication Or Divide Result Registers (High)", Address = 0x4217, LabelShort = "RDMPYH", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 1 - Low)", Address = 0x4218, LabelShort = "JOY1L", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 1 - High)", Address = 0x4219, LabelShort = "JOY1H", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 2 - Low)", Address = 0x421A, LabelShort = "JOY2L", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 2 - High)", Address = 0x421B, LabelShort = "JOY2H", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 3 - Low)", Address = 0x421C, LabelShort = "JOY3L", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 3 - High)", Address = 0x421D, LabelShort = "JOY3H", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 4 - Low)", Address = 0x421E, LabelShort = "JOY4L", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" },
        new RegisterSection { Label = "Controller Port Data Registers (Pad 4 - High)", Address = 0x421F, LabelShort = "JOY4H", Style = "single", Access = "read", Timing = "any time that is not auto-joypad" }
    };

        public static readonly List<RegisterSection> PPU_Registers = new()
    {
        new RegisterSection { Label = "Screen Display Register", Address = 0x2100, LabelShort = "INIDISP", Style = "single ", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "Object Size and Character Size Register", Address = 0x2101, LabelShort = "OBSEL", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "OAM Address Registers (Low)", Address = 0x2102, LabelShort = "OAMADDL", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "OAM Address Registers (High)", Address = 0x2103, LabelShort = "OAMADDH", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "OAM Data Write Register", Address = 0x2104, LabelShort = "OAMDATA", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Mode and Character Size Register", Address = 0x2105, LabelShort = "BGMODE", Style = "single ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mosaic Register", Address = 0x2106, LabelShort = "MOSAIC", Style = "single ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Tilemap Address Registers (BG1)", Address = 0x2107, LabelShort = "BG1SC", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Tilemap Address Registers (BG2)", Address = 0x2108, LabelShort = "BG2SC", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Tilemap Address Registers (BG3)", Address = 0x2109, LabelShort = "BG3SC", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Tilemap Address Registers (BG4)", Address = 0x210A, LabelShort = "BG4SC", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Character Address Registers (BG1&2)", Address = 0x210B, LabelShort = "BG12NBA", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Character Address Registers (BG3&4)", Address = 0x210C, LabelShort = "BG34NBA", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG1 Horizontal)", Address = 0x210D, LabelShort = "BG1HOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG1 Vertical)", Address = 0x210E, LabelShort = "BG1VOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG2 Horizontal)", Address = 0x210F, LabelShort = "BG2HOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG2 Vertical)", Address = 0x2110, LabelShort = "BG2VOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG3 Horizontal)", Address = 0x2111, LabelShort = "BG3HOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG3 Vertical)", Address = 0x2112, LabelShort = "BG3VOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG4 Horizontal)", Address = 0x2113, LabelShort = "BG4HOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "BG Scroll Registers (BG4 Vertical)", Address = 0x2114, LabelShort = "BG4VOFS", Style = "dual ", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Video Port Control Register", Address = 0x2115, LabelShort = "VMAIN", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Address Registers (Low)", Address = 0x2116, LabelShort = "VMADDL", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Address Registers (High)", Address = 0x2117, LabelShort = "VMADDH", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Data Write Registers (Low)", Address = 0x2118, LabelShort = "VMDATAL", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Data Write Registers (High)", Address = 0x2119, LabelShort = "VMDATAH", Style = "single ", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "Mode 7 Settings Register", Address = 0x211A, LabelShort = "M7SEL", Style = "single", Access = "write", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x211B, LabelShort = "M7A", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x211C, LabelShort = "M7B", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x211D, LabelShort = "M7C", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x211E, LabelShort = "M7D", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x211F, LabelShort = "M7X", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Mode 7 Matrix Registers", Address = 0x2120, LabelShort = "M7Y", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "CGRAM Address Register", Address = 0x2121, LabelShort = "CGADD", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "CGRAM Data Write Register", Address = 0x2122, LabelShort = "CGDATA", Style = "dual", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Settings Registers", Address = 0x2123, LabelShort = "W12SEL", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Settings Registers", Address = 0x2124, LabelShort = "W34SEL", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Settings Registers", Address = 0x2125, LabelShort = "WOBJSEL", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Position Registers (WH0)", Address = 0x2126, LabelShort = "WH0", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Position Registers (WH1)", Address = 0x2127, LabelShort = "WH1", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Position Registers (WH2)", Address = 0x2128, LabelShort = "WH2", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Position Registers (WH3)", Address = 0x2129, LabelShort = "WH3", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Logic registers (BG)", Address = 0x212A, LabelShort = "WBGLOG", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Logic registers (OBJ)", Address = 0x212B, LabelShort = "WOBJLOG", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Screen Destination Registers", Address = 0x212C, LabelShort = "TM", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Screen Destination Registers", Address = 0x212D, LabelShort = "TS", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Destination Registers", Address = 0x212E, LabelShort = "TMW", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Window Mask Destination Registers", Address = 0x212F, LabelShort = "TSW", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Color Math Registers", Address = 0x2130, LabelShort = "CGWSEL", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Color Math Registers", Address = 0x2131, LabelShort = "CGADSUB", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Color Math Registers", Address = 0x2132, LabelShort = "COLDATA", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Screen Mode Select Register", Address = 0x2133, LabelShort = "SETINI", Style = "single", Access = "write", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Multiplication Result Registers", Address = 0x2134, LabelShort = "MPYL", Style = "single", Access = "read", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Multiplication Result Registers", Address = 0x2135, LabelShort = "MPYM", Style = "single", Access = "read", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Multiplication Result Registers", Address = 0x2136, LabelShort = "MPYH", Style = "single", Access = "read", Timing = "f-blank, v-blank, h-blank" },
        new RegisterSection { Label = "Software Latch Register", Address = 0x2137, LabelShort = "SLHV", Style = "single", Access = "", Timing = "any time" },
        new RegisterSection { Label = "OAM Data Read Register", Address = 0x2138, LabelShort = "OAMDATAREAD", Style = "dual", Access = "read", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Data Read Register (Low)", Address = 0x2139, LabelShort = "VMDATALREAD", Style = "single", Access = "read", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "VRAM Data Read Register (High)", Address = 0x213A, LabelShort = "VMDATAHREAD", Style = "single", Access = "read", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "CGRAM Data Read Register", Address = 0x213B, LabelShort = "CGDATAREAD", Style = "dual", Access = "read", Timing = "f-blank, v-blank" },
        new RegisterSection { Label = "Scanline Location Registers (Horizontal)", Address = 0x213C, LabelShort = "OPHCT", Style = "dual", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "Scanline Location Registers (Vertical)", Address = 0x213D, LabelShort = "OPVCT", Style = "dual", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "PPU Status Register", Address = 0x213E, LabelShort = "STAT77", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "PPU Status Register", Address = 0x213F, LabelShort = "STAT78", Style = "single", Access = "read", Timing = "any time" },
        new RegisterSection { Label = "WRAM Data Register", Address = 0x2180, LabelShort = "WMDATA", Style = "single", Access = "both", Timing = "any time" },
        new RegisterSection { Label = "WRAM Address Registers", Address = 0x2181, LabelShort = "WMADDL", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "WRAM Address Registers", Address = 0x2182, LabelShort = "WMADDM", Style = "single", Access = "write", Timing = "any time" },
        new RegisterSection { Label = "WRAM Address Registers", Address = 0x2183, LabelShort = "WMADDH", Style = "single", Access = "write", Timing = "any time" }
    };

        public static readonly List<RegisterSection> HDMA_Registers = new List<RegisterSection>();

        static RegistersList()
        {
            for (int i = 0; i < 8; i++)
            {
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Control Register", Address = Convert.ToInt32("43" + i + "0", 16), LabelShort = "DMAP" + i });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Destination Register", Address = Convert.ToInt32("43" + i + "1", 16), LabelShort = "BBAD" + i });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Table Address Registers", Address = Convert.ToInt32("43" + i + "2", 16), LabelShort = "A1T" + i + "L" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Table Address Registers", Address = Convert.ToInt32("43" + i + "3", 16), LabelShort = "A1T" + i + "H" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Table Address Registers", Address = Convert.ToInt32("43" + i + "4", 16), LabelShort = "A1B" + i });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Indirect Address Registers", Address = Convert.ToInt32("43" + i + "5", 16), LabelShort = "DAS" + i + "L" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Indirect Address Registers", Address = Convert.ToInt32("43" + i + "6", 16), LabelShort = "DAS" + i + "H" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Indirect Address Registers", Address = Convert.ToInt32("43" + i + "7", 16), LabelShort = "DASB" + i });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Mid Frame Table Address Registers (Low)", Address = Convert.ToInt32("43" + i + "8", 16), LabelShort = "A2A" + i + "L" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Mid Frame Table Address Registers (High)", Address = Convert.ToInt32("43" + i + "9", 16), LabelShort = "A2A" + i + "H" });
                HDMA_Registers.Add(new RegisterSection { Label = "HDMA Line Counter Register", Address = Convert.ToInt32("43" + i + "A", 16), LabelShort = "NTLR" + i });
            }
        }


       
       
                 

    }
}