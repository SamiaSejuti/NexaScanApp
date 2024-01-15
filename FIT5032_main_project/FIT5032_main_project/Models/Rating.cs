using FIT5032_main_project.Models;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; }  
    public string PatientId { get; set; }  
    public virtual Patient Patient { get; set; }

    public int DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
}
