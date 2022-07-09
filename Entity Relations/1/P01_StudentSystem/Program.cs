namespace P01_StudentSystem
{
    using System;
    using P01_StudentSystem.Data;

    public class StartUp
    {
        public static void Main()
        {
            var db = new StudentSystemDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}