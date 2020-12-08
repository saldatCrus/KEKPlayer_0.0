using System.Drawing;

namespace KEKPlayer.Messages
{
    class ImageMessage : IMessage
    {      
        public ImageMessage(Bitmap compostionImage)
        {
            CompostionImage = compostionImage;
        }

        public Bitmap CompostionImage { get; set; }
    
    }
}
