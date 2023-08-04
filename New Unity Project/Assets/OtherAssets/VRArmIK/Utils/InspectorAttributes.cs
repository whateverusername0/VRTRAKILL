using UnityEngine;

public class DisplayOnlyAttribute : PropertyAttribute
{
}
public class LabelOverride : PropertyAttribute
{
	public string label;
	public LabelOverride(string label)
	{
		this.label = label;
	}

}