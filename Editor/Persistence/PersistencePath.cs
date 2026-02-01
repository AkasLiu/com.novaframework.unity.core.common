/// -------------------------------------------------------------------------------
/// Copyright (C) 2025, Hurley, Independent Studio.
/// Copyright (C) 2025 - 2026, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// -------------------------------------------------------------------------------

using System.IO;

using UnityEditor;

namespace NovaFramework.Editor
{
    /// <summary>
    /// 持久化数据的路径管理辅助工具类
    /// </summary>
    public static class PersistencePath
    {
        /// <summary>
        /// 通用模块的本地包名
        /// </summary>
        public const string LocalPackageNameOfCommonModule = @"com.novaframework.unity.core.common";

        /// <summary>
        /// Nova框架基础文件夹的本地安装路径
        /// </summary>
        public const string LocalInstallPathOfNovaFrameworkDataFolder = @"Assets/../NovaFrameworkData/";
        /// <summary>
        /// Nova框架仓库文件夹的本地安装路径
        /// </summary>
        public const string LocalInstallPathOfNovaFrameworkRepositoryFolder = @"Assets/../NovaFrameworkData/framework_repo/";

        /// <summary>
        /// 获取指定模块的外部仓库地址
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <returns>返回模块的外部仓库路径</returns>
        public static string ExternalRepositoryUrlOfTargetModule(string module)
        {
            return LocalInstallPathOfNovaFrameworkRepositoryFolder + module;
        }

        /// <summary>
        /// 获取指定模块当前使用的仓库地址
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <returns>返回模块的当前文件路径</returns>
        public static string CurrentUsingRepositoryUrlOfTargetModule(string module)
        {
            string url = ExternalRepositoryUrlOfTargetModule(module);

            if (Directory.Exists(url))
            {
                return url;
            }

            // 使用AssetDatabase查找模块文件夹
            return FindModuleFolderPathUsingAssetDatabase(module);
        }

        /// <summary>
        /// 使用AssetDatabase查找指定模块的文件夹路径
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <returns>返回有效的模块文件夹路径</returns>
        private static string FindModuleFolderPathUsingAssetDatabase(string module)
        {
            // 使用AssetDatabase查找所有包含指定模块名称的文件夹路径
            string[] guids = AssetDatabase.FindAssets(module, null);

            for (int n = 0; null != guids && n < guids.Length; ++n)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[n]);
                // 检查是否是文件夹而不是其他资产类型
                if (Directory.Exists(path))
                {
                    // 检查文件夹名称是否完全匹配
                    if (Path.GetFileName(path) == module)
                    {
                        return path;
                    }
                }
            }

            // 未找到资源
            return null;
        }

        /// <summary>
        /// 仓库资源清单文件的绝对路径
        /// </summary>
        internal static readonly string AbsolutePathOfRepositoryManifestFile = Path.Combine(CurrentUsingRepositoryUrlOfTargetModule(LocalPackageNameOfCommonModule), "Editor Default Resources/Config/repo_manifest.xml").Replace("\\", "/");
    }
}
