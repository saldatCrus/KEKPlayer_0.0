using System.Drawing;

namespace KEKPlayer.Messages
{
    class ImageMessage : IMessage
    {      
        public ImageMessage(Image compostionImage)
        {
            CompostionImage = compostionImage;
        }

        public Image CompostionImage { get; set; }
    
    }
}
