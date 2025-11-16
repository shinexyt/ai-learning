namespace AIStudySite.Cms.Services;

public class LocalizedTextService
{
    private readonly Dictionary<string, (string en, string zh)> _resources = new(StringComparer.OrdinalIgnoreCase)
    {
        ["home.title"] = ("AI Learning Map", "AI学习地图"),
        ["home.headline"] = ("AI Knowledge Tree", "AI知识树"),
        ["home.subheadline"] = ("Browse curated nodes that cover modern AI topics.", "浏览覆盖现代AI主题的知识节点。"),
        ["nav.home"] = ("Home", "首页"),
        ["nav.articles"] = ("Tutorials", "教程"),
        ["nav.pro"] = ("Paid Zone", "付费专区"),
        ["nav.login"] = ("Login", "登录"),
        ["nav.register"] = ("Register", "注册"),
        ["nav.language"] = ("Lang", "语言"),
        ["nav.footer"] = ("Content managed by Orchard Core CMS.", "由Orchard Core CMS管理内容。"),
        ["nav.tagline"] = ("AI upskilling portal", "AI 进阶学习入口"),
        ["articles.title"] = ("Tutorial Articles", "教程文章"),
        ["articles.headline"] = ("Learn from curated tutorials", "学习精选教程"),
        ["articles.description"] = ("Filter by knowledge node or browse all.", "按知识节点筛选或浏览全部。"),
        ["articles.empty"] = ("No tutorials found for this filter.", "此筛选下暂无教程。"),
        ["articles.node"] = ("Node", "知识点"),
        ["articles.read"] = ("Read", "阅读"),
        ["articles.notfound"] = ("We could not find that article.", "未找到该文章。"),
        ["loading"] = ("Loading…", "加载中…"),
        ["pro.title"] = ("Premium Resources", "高级资源"),
        ["pro.headline"] = ("Exclusive AI Lab Content", "专属AI实验室内容"),
        ["pro.description"] = ("Only members in the PaidUser role can access this area.", "仅 PaidUser 角色可访问该区域。"),
        ["pro.notice.title"] = ("Coming soon", "即将上线"),
        ["pro.notice.description"] = ("Upgrade your plan to unlock community projects, datasets, and code walkthroughs.", "升级计划即可解锁社区项目、数据集与代码讲解。")
    };

    public string Get(string key, string culture)
    {
        if (!_resources.TryGetValue(key, out var tuple))
        {
            return key;
        }

        return culture.Equals("zh-CN", StringComparison.OrdinalIgnoreCase) ? tuple.zh : tuple.en;
    }
}
