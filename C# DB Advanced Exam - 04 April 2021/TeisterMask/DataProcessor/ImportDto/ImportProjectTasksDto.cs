namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    using System.ComponentModel.DataAnnotations;

    using Shared;


    [XmlType("Task")]
    public class ImportProjectTasksDto
    {
        [XmlElement("Name")]
        [MaxLength(GlobalConstants.TaskNameMaxLength)]
        [MinLength(GlobalConstants.TaskNameMinLength)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string TaskOpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string TaskDueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(GlobalConstants.TaskExecutionTypeMinLenght, GlobalConstants.TaskExecutionTypeMaxLenght)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(GlobalConstants.TaskLabelTypeMinLenght, GlobalConstants.TaskLabelTypeMaxLenght)]
        public int LabelType { get; set; }


    }
}
