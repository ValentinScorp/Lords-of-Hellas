using System;

public interface IRegularAction
{
    void Done();
    void Undo();
    void Cancel();
}
