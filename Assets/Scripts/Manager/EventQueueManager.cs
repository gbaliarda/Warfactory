using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQueueManager : Singleton<EventQueueManager>
{
    public List<ICommand> Events => _events;
    private List<ICommand> _events = new();
    
    public List<ICommand> UndoEvents => _undoEvents;
    private List<ICommand> _undoEvents = new();

    public Queue<ICommand> EventQueue => _eventQueue;
    private Queue<ICommand> _eventQueue = new();

    private void Update()
    {
        foreach (var command in _events)
        {
            command.Execute();
        }

        foreach (var command in _undoEvents)
        {
            command.Undo();
        }

        foreach (var command in _eventQueue)
        {
            command.Execute();
        }


        _events.Clear();
        _undoEvents.Clear();
        _eventQueue.Clear();
    }

    public void AddEvent(ICommand command) => _events.Add(command);
    public void AddEventToQueue(ICommand command) => _eventQueue.Enqueue(command);
    public void AddUndoEvent(ICommand command) => _undoEvents.Add(command);
}
