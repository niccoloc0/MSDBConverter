using System;
using System.Text;
using System.Threading;

public class SimpleConsoleProgressBar
{
    private const int BlockCount = 30;
    private const string Animation = @"|/-\";
    private int _animationIndex = 0;
    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);
    private DateTime _nextAnimationUpdateTime = DateTime.MinValue;

    private string _currentText = string.Empty;

    public void Draw(int progress, int total, string statusText = "")
    {
        if (total <= 0) total = 1;
        if (progress < 0) progress = 0;
        if (progress > total) progress = total;

        double percent = (double)progress / total;
        int numBlocks = (int)(percent * BlockCount);

        Console.CursorLeft = 0;

        StringBuilder sb = new StringBuilder("[");
        sb.Append('#', numBlocks);
        sb.Append('-', BlockCount - numBlocks);
        sb.Append("] ");

        sb.AppendFormat("{0,3}% ", (int)(percent * 100));

        if (DateTime.UtcNow > _nextAnimationUpdateTime)
        {
            _animationIndex = (_animationIndex + 1) % Animation.Length;
            _nextAnimationUpdateTime = DateTime.UtcNow + _animationInterval;
        }
        sb.Append(Animation[_animationIndex]);

        if (!string.IsNullOrWhiteSpace(statusText))
        {
            sb.Append(" ");
            sb.Append(statusText);
        }

        string newText = sb.ToString();
        Console.Write(newText);

        if (_currentText.Length > newText.Length && _currentText.StartsWith("["))
        {
            Console.Write(new string(' ', _currentText.Length - newText.Length));
        }
        _currentText = newText;

        if (progress == total)
        {
            Console.WriteLine();
            _currentText = string.Empty;
        }
    }

    public void Finish(string finishText = "Done!")
    {
        Draw(BlockCount, BlockCount, finishText);
        Console.WriteLine();
        _currentText = string.Empty;
    }
    public string GetCurrentText() => _currentText;
}