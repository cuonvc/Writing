namespace Writing.Handle; 

public class FileHandler {
    
    public string generatePath(IFormFile file, int id, string dir) {
        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string currentDir = Directory.GetCurrentDirectory();
        string relativePath = Path.Combine($"\\resource\\images\\{dir}", id.ToString(), newFileName);
        Directory.CreateDirectory(Path.Combine($"resource/images/{dir}", id.ToString()));  //create directory if not existed
        string absolutePath = Path.ChangeExtension(currentDir + relativePath, "png").Replace(".jpg", ".png")
            .Replace(".jpeg", "png");
        foreach (var f in Directory.GetFiles(currentDir + $"/resource/images/{dir}/" + id.ToString())) {
            File.Delete(f);
        }
        using (var fileStream = new FileStream(absolutePath, FileMode.Create)) {
            file.CopyTo(fileStream);
        }

        return relativePath.Substring(1).Replace("\\", "%2F").Replace(".jpg", ".png");  //%2F == /
    }
}