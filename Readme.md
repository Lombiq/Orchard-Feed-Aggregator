# Feed Aggregator



## About

Orchard module for aggregating feeds by creating content items from them. Supported feed types are: RSS and Atom feeds. This module was created when developing the new Orchard app driving [www.dotnetfoundation.org](http://www.dotnetfoundation.org/), the website of .NET Foundation. It's also available for all sites on [DotNest, the Orchard SaaS](https://dotnest.com).


## Feed requirements

### Rss

- Feed items must contain a "pubDate" node.
- Feed items must contain a "guid", "title" or "description" node.
- Feeds must have an "rss" root node.

### Atom

- Feed entries must contain an "updated" node.
- Feed entries must contain an "id" node.
- Feeds must use the http://www.w3.org/2005/Atom namespace in the "feed" root node.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.