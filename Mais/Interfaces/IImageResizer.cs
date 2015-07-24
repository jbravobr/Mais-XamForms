using System;

namespace Mais
{
	public interface IImageResizer
	{
		byte[] ResizeImage(byte[] imageData, float width, float height);
		// Banner 640x325
	}
}

