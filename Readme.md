# Feed Aggregator Readme



## Project Description

Orchard module for aggregating feeds by creating content items from them. Supported feed types are: RSS and Atom feeds.

### Feed requirements

#### Rss

- Feed items must contain a "pubDate" node.
- Feed items must contain a "guid", "title" or "description" node.
- Feeds must have an "rss" root node.

#### Atom

- Feed entries must contain an "updated" node.
- Feed entries must contain an "id" node.
- Feeds must use the http://www.w3.org/2005/Atom namespace in the "feed" root node.