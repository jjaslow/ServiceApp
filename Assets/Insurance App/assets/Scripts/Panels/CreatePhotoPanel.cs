using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePhotoPanel : MonoBehaviour, iPanel
{
    public Text caseNumberText;
    public Button photoImage;
    public InputField photoText;
	public Sprite originalButtonImage;

	Sprite mySprite;
	[SerializeField]
	Sprite defaultSprite;

	bool tookPhoto = false;

	private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }


    public void TakePhoto()
    {
		TakePicture(1024);
		
	}

	public void ProcessInfo()
    {
		if (!tookPhoto)
		{	//insert defaul image
			mySprite = defaultSprite;
			photoImage.GetComponent<Image>().sprite = mySprite;
		}
			

		//save photo and notes to case
		UIManager.Instance.activeCase.photo = spriteToByteArray(mySprite);
		UIManager.Instance.activeCase.photoNotes = photoText.text;

		UIManager.Instance.createOverviewPanel.SetActive(true);
	}


	private void TakePicture(int maxSize)
	{
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			Debug.Log("Photo Image path: " + path);
			if (path != null)
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
				
				if (texture == null)
				{
					Debug.Log("Camera Couldn't load texture from " + path);
					return;
				}
				else
				{
					mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
					photoImage.GetComponent<Image>().sprite = mySprite;
					tookPhoto = true;
				}

			}
		}, maxSize);

		Debug.Log("Camera Permission result: " + permission);

	}


	private byte[] spriteToByteArray(Sprite mySprite)
	{
		return mySprite.texture.EncodeToJPG();

	}

	public void ResetCameraButtomImage()
	{
		photoImage.GetComponent<Image>().sprite = originalButtonImage;

	}

}
