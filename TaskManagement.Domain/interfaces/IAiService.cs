using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.interfaces;
public interface IAiService
{
    Task<string> GenerateSummary(string text);
    Task<string> DetectPriority(string text);
    Task<string> CategorizeTask(string text);
    Task<List<string>> GenerateTasks(string prompt);
    Task<string> Chat(string prompt);
}
