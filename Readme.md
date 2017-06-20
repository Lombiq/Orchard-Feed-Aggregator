# Feed aggregator readme



## Project description

Orchard module for aggregating feeds by creating content items from them. Supported feed types are: RSS and Atom feeds. This module was created when developing the new Orchard app driving [www.dotnetfoundation.org](http://www.dotnetfoundation.org/), the website of .NET Foundation. It's also available for all sites on [DotNest, the Orchard SaaS](https://dotnest.com).

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-feed-aggregator](https://bitbucket.org/Lombiq/orchard-feed-aggregator) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Feed-Aggregator](https://github.com/Lombiq/Orchard-Feed-Aggregator) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](https://lombiq.com/). Commercial-grade support is available through Lombiq.


## Feed requirements

### Rss

- Feed items must contain a "pubDate" node.
- Feed items must contain a "guid", "title" or "description" node.
- Feeds must have an "rss" root node.

### Atom

- Feed entries must contain an "updated" node.
- Feed entries must contain an "id" node.
- Feeds must use the http://www.w3.org/2005/Atom namespace in the "feed" root node.