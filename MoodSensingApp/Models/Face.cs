using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoodSensingApp.Models
{
	public class Face
	{
		public Guid FaceId { get; set; }
		public FaceRectangle FaceRectangle { get; set; }
		public FaceLandmarks FaceLandmarks { get; set; }
		public FaceAttributes FaceAttributes { get; set; }
	}

	public class FaceRectangle
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public int Left { get; set; }
		public int Top { get; set; }
	}

	public class FeatureCoordinate
	{
		public double X { get; set; }
		public double Y { get; set; }
	}

	//public class FeatureCoordinate
	//{
	//	public double X { get; set; }
	//	public double Y { get; set; }
	//}

	public class FaceLandmarks
	{
		public FeatureCoordinate PupilLeft { get; set; }
		public FeatureCoordinate PupilRight { get; set; }
		public FeatureCoordinate NoseTip { get; set; }
		public FeatureCoordinate MouthLeft { get; set; }
		public FeatureCoordinate MouthRight { get; set; }
		public FeatureCoordinate EyebrowLeftOuter { get; set; }
		public FeatureCoordinate EyebrowLeftInner { get; set; }
		public FeatureCoordinate EyeLeftOuter { get; set; }
		public FeatureCoordinate EyeLeftTop { get; set; }
		public FeatureCoordinate EyeLeftBottom { get; set; }
		public FeatureCoordinate EyeLeftInner { get; set; }
		public FeatureCoordinate EyebrowRightInner { get; set; }
		public FeatureCoordinate EyebrowRightOuter { get; set; }
		public FeatureCoordinate EyeRightInner { get; set; }
		public FeatureCoordinate EyeRightTop { get; set; }
		public FeatureCoordinate EyeRightBottom { get; set; }
		public FeatureCoordinate EyeRightOuter { get; set; }
		public FeatureCoordinate NoseRootLeft { get; set; }
		public FeatureCoordinate NoseRootRight { get; set; }
		public FeatureCoordinate NoseLeftAlarTop { get; set; }
		public FeatureCoordinate NoseRightAlarTop { get; set; }
		public FeatureCoordinate NoseLeftAlarOutTip { get; set; }
		public FeatureCoordinate NoseRightAlarOutTip { get; set; }
		public FeatureCoordinate UpperLipTop { get; set; }
		public FeatureCoordinate UpperLipBottom { get; set; }
		public FeatureCoordinate UnderLipTop { get; set; }
		public FeatureCoordinate UnderLipBottom { get; set; }
	}

	public enum FaceAttributeType
	{
		Age,
		Gender,
		HeadPose,
		Smile,
		FacialHair,
		Glasses,
		Emotion,
		Hair,
		Makeup,
		Occlusion,
		Accessories,
		Blur,
		Exposure,
		Noise
	}

	public class FaceAttributes
	{
		public double Age { get; set; }
		public string Gender { get; set; }
		public HeadPose HeadPose { get; set; }
		public double Smile { get; set; }
		public FacialHair FacialHair { get; set; }
		public EmotionScores Emotion { get; set; }
		[JsonProperty("glasses")]
		[System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
		public Glasses Glasses { get; set; }
		public Blur Blur { get; set; }
		//public Exposure Exposire { get; set; }
		public Noise Noise { get; set; }
		public Makeup Makeup { get; set; }
		//public Accessory[] Accessories { get; set; }
		public Occlusion Occlusion { get; set; }
		public FacialHair Hair { get; set; }
	}
	[System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
	public enum BlurLevel
	{
		Low,
		Medium,
		High
	}

	public class Blur
	{
		public BlurLevel BlurLevel { get; set; }
		public double Value { get; set; }
	}
	public enum Glasses
	{
		NoGlasses,
		Sunglasses,
		ReadingGlasses,
		SwimmingGoogles
	}
	public class Makeup
	{
		public bool EyeMakeup { get; set; }
		public bool LipMakeup { get; set; }
	}

	[System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
	public enum NoiseLevel
	{
		Low,
		Medium,
		High
	}

	public class Noise
	{
		public NoiseLevel NoiseLevel { get; set; }
		public double Value { get; set; }
	}
	public class Occlusion
	{
		public bool ForeheadOccluded { get; set; }
		public bool EyeOccluded { get; set; }
		public bool MouthOccluded { get; set; }
	}
	public class HeadPose
	{
		public double Roll { get; set; }
		public double Yaw { get; set; }
		public double Pitch { get; set; }
	}

	public class FacialHair
	{
		public double Moustache { get; set; }
		public double Beard { get; set; }
		public double Sideburns { get; set; }
	}
}
