namespace Lennox.NvEncSharp
{
    // ReSharper disable UnusedMember.Global
    public static class NvEncRegisterResourceEx
    {
        public static NvEncInputPtr AsInputPointer(
            this NvEncRegisterResource resource)
        {
            return new NvEncInputPtr
            {
                Handle = resource.RegisteredResource.Handle
            };
        }

        public static NvEncOutputPtr AsOutputPointer(
            this NvEncRegisterResource resource)
        {
            return new NvEncOutputPtr
            {
                Handle = resource.RegisteredResource.Handle
            };
        }
    }
}