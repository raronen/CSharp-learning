using Autofac;
using Decorator.DI;

namespace Decorator.DI {
    public interface IReportingService {
        void Report();
    }

    public class ReportingService : IReportingService {
        public void Report() {
            System.Console.WriteLine("Here is your report");
        }
    }

    public class ReportingServiceWithLogging: IReportingService {
        private IReportingService decorated;

        public ReportingServiceWithLogging(IReportingService decorated) {
            this.decorated = decorated;
        }

        public void Report() {
            System.Console.WriteLine("Commencing log...");
            decorated.Report();
            System.Console.WriteLine("Ending log...");
        }
    }
    
    public class Demo {
        public static void Run() {
            var cb = new ContainerBuilder();
            cb.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            cb.RegisterDecorator<IReportingService>(
                (c, inner) => new ReportingServiceWithLogging(inner),
                fromKey: "reporting"
            );

            using (var c = cb.Build()) {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }
        }
    }
}