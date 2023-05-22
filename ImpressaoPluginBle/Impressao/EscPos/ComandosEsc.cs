namespace ImpressaoPluginBle.Negocio.Impressao.EscPos
{
    public static class ComandosEsc
    {
        //byte[] selectBitImgMode = { 0x1B, 0x2A };
        //private byte[] setLineSpacing = { 0x1B, 0x33, 0x0D };// Quebra de linha
        public static byte[] setLineSpacing = { 0x1B, 0x32, 0x0D };// Quebra de linha
        public static byte[] printAndFeedPaper = { 0x0A };// Imprimir exibir mais papel/ ou parar de tirar papel

        //private byte[] backspace = new byte[] { 0x1D, 0x56, 0x41, 0x00 };//{ 0x08 };// Quebra de linha
        //private byte[] traco_ = new byte[] { 0x1B, 0x86 };//{ 0x08 };// Quebra de linha
        //List<byte> lineBuff = new List<byte>();

        // Negrito
        public static byte[] TEXT_BOLD_OFF = { 0x1b, 0x45, 0x00 }; // Bold font OFF
        public static byte[] TEXT_BOLD_ON = { 0x1b, 0x45, 0x01 };// Bold font ON

        // Alinhamento
        public static byte[] Align_LEFT = { 0x1b, 0x61, 0x00 };
        public static byte[] Align_CENTER = { 0x1b, 0x61, 0x01 };
        public static byte[] Align_RIGHT = { 0x1b, 0x61, 0x02 };

        public static byte[] TEXT_SIZE_NORMAL = { 0x1b, 0x21, 0x00 };
        public static byte[] TEXT_SIZE_LARGE = { 0x1b, 0x21, 0x30, 0x00 };//0x30
        public static byte[] TEXT_SIZE_MEDIO = { 0x1b, 0x21, 0x05, 0x00 };//0x30

        //byte[] PD_P50 = { 0x1d, 0x7c, 0x08 }; // Printing Density +50%

        public static byte[] TEXT_FONT_A = { 0x1b, 0x4d, 0x00 };// Font type A
        public static byte[] TEXT_FONT_B = { 0x1b, 0x4d, 0x01 };// Font type B

        public static byte[] TEXT_FONT_A1 = { 0x1d, 0x66, 0x35 };// Font type A
        public static byte[] TEXT_FONT_B1 = { 0x1d, 0x66, 0x31 };// Font type A

        public static byte[] TAB1 = { 0x09 }; // Horizontal tab
        public static byte[] TAB2 = { 0x1b, 0x44, 0x17 };
        //byte[] TAB3 = { 0x1b, 0x44, 0x03 };
        public static byte[] TAB3 = { 0x1B, 0x44, 0x28, 0x00 };

        public static byte[] QUEBRALINHA = { 0x0A };

    }
}
