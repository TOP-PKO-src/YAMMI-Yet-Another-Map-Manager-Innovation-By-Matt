#nullable disable
namespace YetAnotherMapManager.UI;

internal class PictureComboBoxItem
{
  private string _text;
  private int _imageIndex;

  public string Text
  {
    get => this._text;
    set => this._text = value;
  }

  public int ImageIndex
  {
    get => this._imageIndex;
    set => this._imageIndex = value;
  }

  public PictureComboBoxItem()
    : this("")
  {
  }

  public PictureComboBoxItem(string text)
    : this(text, -1)
  {
  }

  public PictureComboBoxItem(string text, int imageIndex)
  {
    this._text = text;
    this._imageIndex = imageIndex;
  }

  public override string ToString() => this._text;
}
