using System.Reflection;
using XFrame.Common.Extensions;
using XFrame.DomainContainers;

namespace XFrame.Jobs.Extensions
{
    public static class DomainContainerJobExtensions
    {
        public static IDomainContainer AddJobs(
            this IDomainContainer domainContainer,
            params Type[] jobTypes)
        {
            return domainContainer.AddTypes(jobTypes);
        }

        public static IDomainContainer AddJobs(
            this IDomainContainer domainContainer,
            Assembly fromAssembly,
            Predicate<Type> predicate = null)
        {
            predicate = predicate ?? (t => true);
            var jobTypes = fromAssembly
                .GetTypes()
                .Where(type => !type.GetTypeInfo().IsAbstract && type.IsAssignableTo<IJob>())
                .Where(t => !t.HasConstructorParameterOfType(i => i.IsAssignableTo<IJob>()))
                .Where(t => predicate(t));
            return domainContainer.AddTypes(jobTypes);
        }
    }
}
