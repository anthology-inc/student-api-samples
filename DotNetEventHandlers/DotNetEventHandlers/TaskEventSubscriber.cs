using Cmc.Core.Eventing;
using Cmc.Nexus.Crm.Entities;

namespace DotNetEventHandlers
{
    public class TaskEventSubscriber : EventSubscriber
    {
        public override void RegisterHandlers(IEventService eventService)
        {

            //the Saving event will be raised prior to the entity persisting to the database, this provides
            //an opportunity to add validation logic
            eventService.GetEvent<SavingEvent>().RegisterHandler<TaskEntity>((e, a) =>
            {
                if (string.IsNullOrWhiteSpace(e.Note))
                    a.ValidationMessages.Add("You must enter a note.");
            });

            //the Constructed event occurs when a new instance of this entity is created, this provides
            //an opportunity to apply defaults to the entity
            eventService.GetEvent<ConstructedEvent>().RegisterHandler<TaskEntity>((e, a) =>
            {
                e.Note = "Please enter a note";
            });
        }
    }
}
