namespace FoodMVCWebApp.Interfaces
{
    public interface IImageService
	{
		public Task Upload(IFormFile imgFile);
        public Task<string> GetStoragePath();
    }
}

