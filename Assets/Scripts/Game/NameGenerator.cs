using UnityEngine;
using System.Collections;

public class NameGenerator : MonoBehaviour {

	private static string corpName = "MEGACORP";
	private static string corpSuffix = "Test";

	private static string[] namePrefix = {"Mecha", "Tecno", "Beam", "Mega", "Cyber", "Bio", "Hyper", "Ultra", "Nano", "Future", "Data", "Techno", "Neo", "Ion", "Xeno", "Zen", "Compu", "Spectra", "Ray", "Laser", "Pulse", "Astro", "Meta", "Micro", "Macro", "Alpha", "Life"};
	private static string[] nameSuffix = {"corp", "dyne", "tech", "lab", "labs", "space", "cell", "tec", "tek", "search", "works", "droid", "ways", "wave", "world", "terrestrial", "storm"};

	private static string[] institutePrefix = {"Center for", "Institute of", "Division of", "Facility for"};

	private static string[] bizPrefix = {"Dynamic", "Applied", "Technological", "Logical", "Conceptual", "Theoretical", "Scalar", "Numerical", "Recreational", "Professional", "Granular", "Quantum", "Logistic", "Experimental", "Fringe", "Esoteric", "Mystic", "Occult", "Paranormal", "Extraterrestrial", "Planetary", "Transdimensional", "Interdimensional", "Multiplanar"};
	private static string[] bizSuffix = {"Innovations", "Research", "Manufacturing", "Industries", "Systems", "Mechanics", "Ventures", "Global", "Propulsion", "Imports & Exports", "Electronics", "Arts", "Engineering", "Technology", "Theory", "Control", "Relations"};
	
	private static string[] familyNames = {"Tessier", "Ashpool", "Southgrove", "Lakeberg", "Maas", "Aalderen", "Williams", "Grayson", "Williamson", "Hawkins", "Hawkings", "Fourier", "Gently"};
	
	private static string[] incList = {"LLC", "Inc.", "Ltd.", "GmbH", "AB", "Oy", "A/S", "International"};

	public static string GenerateCorpName () {
		string o = "";
		
		if (Random.Range(0f, 1f) > 0.5f) {
			o += namePrefix[Random.Range(0, namePrefix.Length)] + nameSuffix[Random.Range(0, nameSuffix.Length)] + " ";
		} else {
			o += familyNames[Random.Range(0, familyNames.Length)] + "-" + familyNames[Random.Range(0, familyNames.Length)] + " ";
		}

		if (Random.Range(0f, 1f) > 0.5f) o += bizPrefix[Random.Range(0, bizPrefix.Length)] + " ";
		if (Random.Range(0f, 1f) > 0.5f) o += bizSuffix[Random.Range(0, bizSuffix.Length)] + " ";
		if (Random.Range(0f, 1f) > 0.5f) o += incList[Random.Range(0, incList.Length)] + " ";

		return o;
		
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			print(GenerateCorpName());	
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
