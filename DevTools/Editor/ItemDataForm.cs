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
            AddField(new EnumInputField("Item Type", typeof(ItemType)));
        }

        internal void SetValues(Item item)
        {
            fields[0].Value.Append(item.name);
            fields[1].Value.Append(item.description);
            fields[2].Value.Append(item.value);
            fields[3].Value.Append(item.GetItemType());
        }
    }
}
