namespace KEKPlayer.Messages
{
    class ImageMessage : IMessage
    {
        public ImageMessage(string Source,string Name )
        {
            ImageSource = Source;

            ImageName = Name;
        }

        public string ImageSource { get; set; }

        public string ImageName { get; set; }

    }
}
