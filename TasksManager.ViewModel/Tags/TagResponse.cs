namespace TasksManager.ViewModel.Tags
{
    public class TagResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TasksCount { get; set; }
        public int? OpenTasksCount { get; set; }
    }
}
