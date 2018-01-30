﻿// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Nuke.Core.OutputSinks;
using Nuke.Core.Utilities;

namespace Nuke.Core.Execution
{
    internal static class BuildExecutor
    {
        public static int Execute<T> (Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            try
            {
                var executionList = Setup(defaultTargetExpression);
                return new ExecutionListRunner().Run(executionList);
            }
            catch (Exception exception)
            {
                OutputSink.Error(exception.Message, exception.StackTrace);
                return -exception.Message.GetHashCode();
            }
        }

        private static IReadOnlyCollection<TargetDefinition> Setup<T> (Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            PrintLogo();

            var build = CreateBuildInstance(defaultTargetExpression);
            
            HandleGraphAndHelp(build);

            var executionList = TargetDefinitionLoader.GetExecutionList(build);
            RequirementService.ValidateRequirements(executionList, build);
            return executionList;
        }

        private static void HandleGraphAndHelp<T> (T build)
                where T : NukeBuild
        {
            if (build.Help == null)
                return;

            if (build.Help.Length == 0 || build.Help.Any(x => "targets".StartsWithOrdinalIgnoreCase(x)))
                Logger.Log(HelpTextService.GetTargetsText(build));

            if (build.Help.Length == 0 || build.Help.Any(x => "parameters".StartsWithOrdinalIgnoreCase(x)))
                Logger.Log(HelpTextService.GetParametersText(build));

            if (build.Graph)
                GraphService.ShowGraph(build);

            if (build.Help != null || build.Graph)
                Environment.Exit(exitCode: 0);
        }

        private static T CreateBuildInstance<T> (Expression<Func<T, Target>> defaultTargetExpression)
                where T : NukeBuild
        {
            var constructors = typeof(T).GetConstructors();
            ControlFlow.Assert(constructors.Length == 1 && constructors.Single().GetParameters().Length == 0,
                    $"Type '{typeof(T).Name}' must declare a single parameterless constructor.");

            var build = Activator.CreateInstance<T>();
            build.TargetDefinitions = build.GetTargetDefinitions(defaultTargetExpression);
            NukeBuild.Instance = build;
            
            InjectionService.InjectValues(build);

            return build;
        }

        private static void PrintLogo()
        {
            Logger.Log(FigletTransform.GetText("NUKE"));
            Logger.Log(typeof(BuildExecutor).GetTypeInfo().Assembly.GetInformationText());
            Logger.Log();
        }
    }
}
