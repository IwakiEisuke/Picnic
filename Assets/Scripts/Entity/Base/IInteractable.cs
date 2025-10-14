public interface IInteractable
{
    public float Duration { get; }

    /// <summary>
    /// インタラクト可能な状態かどうかを示します。
    /// </summary>
    public bool IsInteractable { get; }

    void Interact();
}
