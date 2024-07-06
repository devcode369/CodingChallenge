namespace ChessGame
{
    public class PieceDetails
    {
        public PieceDetails()
        {
            LinkedPoint = new LinkedList<string>();

        }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public Point? CurrentPoint { get; set; }
        public Image? Img { get; set; }
        public int Value { get; set; }
        public LinkedList<string> LinkedPoint { get; set; }

    }
}
