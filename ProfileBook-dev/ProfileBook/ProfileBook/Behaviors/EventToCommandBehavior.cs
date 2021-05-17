using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<ListView>
    {
        #region Private fields

        private Delegate eventHandler;

        #endregion


        #region Public fields

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        #endregion


        #region Properties

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        #endregion


        #region Overrides

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        #endregion


        #region Private helpers

        private void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't register the {EventName} event");

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }


        private void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            if (eventHandler == null)
                return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't deregister the {EventName} event");

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }


        private void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
                return;

            object resolvedParameter;
            if (CommandParameter != null)
                resolvedParameter = CommandParameter;
            else if (Converter != null)
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            else
                resolvedParameter = eventArgs;
            if (Command.CanExecute(resolvedParameter))
                Command.Execute(resolvedParameter);
        }


        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EventToCommandBehavior behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
                return;

            string oldEventName = (string)oldValue;
            string newName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newName);
        }


        #endregion
    }
}