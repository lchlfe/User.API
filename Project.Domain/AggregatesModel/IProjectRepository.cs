using System.Threading.Tasks;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<Project> GetAsync(int projectId);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task AddAsync(Project project);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task UpdateAsync(Project project);
    }
}