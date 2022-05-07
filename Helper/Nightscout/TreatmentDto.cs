using System.Text.Json.Serialization;

namespace Helper.Nightscout
{

	public class TreatmentDto
    {
		/// <summary>
		/// Internally assigned id.
		/// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? _id { get; set; }

        /// <summary>
        /// The type of treatment event.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// The date of the event, might be set retroactively .
        /// </summary>
        public string Created_at { get; set; }

        /// <summary>
        /// Current glucose.
        /// </summary>
        public string? Glucose { get; set; }

        /// <summary>
        /// Method used to obtain glucose, Finger or Sensor.
        /// </summary>
        public string? GlucoseType { get; set; }

        /// <summary>
        /// Amount of carbs consumed in grams.
        /// </summary>
        public int? Carbs { get; set; }

        /// <summary>
        /// Amount of protein consumed in grams.
        /// </summary>
        public int? Protein { get; set; }

        /// <summary>
        /// Amount of fat consumed in grams.
        /// </summary>
        public int? Fat { get; set; }

        /// <summary>
        /// Amount of insulin, if any.
        /// </summary>
        public double? Insulin { get; set; }

        /// <summary>
        /// The units for the glucose value, mg/dl or mmol.
        /// </summary>
        public string? Units { get; set; }

        /// <summary>
        /// Description/notes of treatment.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Who entered the treatment
        /// </summary>
        public string? EnteredBy { get; set; }
    }
}
