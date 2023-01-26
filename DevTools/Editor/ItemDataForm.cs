using JustRoguelite.Items;

namespace JustRoguelite.Devtools.Editor
{
    public class ItemDataForm : DataForm
    {
        protected override string Header => "Item Data";
        public ItemDataForm()
        {
            AddField(new TextInputField("Name"));
            AddField(new TextInputField("Description"));
            AddField(new NumericInputField("Value"));
        }

        internal void SetValues(Item item)
        {
            fields[0].Value.Append(item.name);
            fields[1].Value.Append(item.description);
            fields[2].Value.Append(item.value);
        }
    }
}