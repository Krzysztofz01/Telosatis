<pre align="center">
 _____                                       _____ 
( ___ )                                     ( ___ )
 |   |~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~|   | 
 |   |                                       |   | 
 |   |  _       _                 _   _      |   | 
 |   | | |_ ___| | ___  ___  __ _| |_(_)___  |   | 
 |   | | __/ _ \ |/ _ \/ __|/ _` | __| / __| |   | 
 |   | | ||  __/ | (_) \__ \ (_| | |_| \__ \ |   | 
 |   |  \__\___|_|\___/|___/\__,_|\__|_|___/ |   | 
 |   |                                       |   | 
 |___|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~|___| 
(_____)                                     (_____)
</pre>


# Telosatis

**⚠️ The project is at an early development stage ⚠️**

A minimalist, browser-based file explorer for developers, DevOps engineers, and system administrators. It is simply a file explorer and nothing more - that is its main strength (at least for me). It solves the problem: "I've deployed the XYZ service; it would be great to have quick access to that service's files via a browser."


## Motivation

I hadn't thought of a name, but I really like these two words, which aptly describe what I expect from a file explorer: [Telos (/ˈtɛlɒs, ˈtiːlɒs/)](https://en.wikipedia.org/wiki/Telos) - the ultimate goal, purpose, or function of a given thing, being, or action; and [satis (/ˈsatis/)](https://en.wiktionary.org/wiki/satis#Latin) - enough, something sufficient.

There are many different options when it comes to browser-based file explorers, but their advantages - namely, a wide range of features and integration options - are, unfortunately, not aligned with the use cases of the projects for which I need this type of solution.

The main factor that contributed to my decision to replace the browser-based file explorer I had been deploying was the suspension of development of the [filebrowser](https://github.com/filebrowser/filebrowser) project. Despite being the primary solution I had been using, it unfortunately became a technological liability due to the need to harden it extensively in response to emerging vulnerabilities, as well as what I consider to be its biggest "anti-feature": the ability to execute commands on the host.

While looking for alternatives, I came across many excellent projects, but unfortunately, my pickiness when it comes to self-hosted solutions meant I couldn't settle on any of them. In terms of features and UI design, [FileGator](https://github.com/filegator/filegator) is a good alternative, but I didn't go with it because, at the time I was under pressure to migrate my running instances, the latest version had a vulnerability for which a fix hadn't yet been released. The vulnerability has, of course, long since been patched, but unfortunately, I had to keep looking.

One project that really surprised me with the author's creativity and the sheer number of features that can be packed into a file browser is [copy-part](https://github.com/9001/copyparty). However, this vast number of features also meant that I didn't choose this solution (I'd have ended up digging through the settings to disable everything I didn't need, one by one).

I was highly intrigued by the [filestash](https://github.com/mickael-kerjean/filestash) solution. I really liked the concept of a universal file system API acting as a facade for various implementations, which could be configured to range from a local file system to cloud solutions and S3-compatible blob storage. Unfortunately, the large number of different API implementations available through the plugin system also fails to meet one of the key principles: third-party dependencies must be kept to a minimum to limit potential attack surfaces. This project, however, served as the inspiration for the working-title solution "telosatis", which also relies on a minimal file system abstraction.


## Features

- Designed and implemented with a focus on minimizing the attack surface, despite being intended for intranet use.
- Access to the native file system is hidden behind a minimal abstract file system, with additional validation to enhance security.
- Access to native operating system paths is provided through a sandbox, which prevents potential path traversal attacks and completely eliminates the risk of command injection.
- A simple authentication mechanism based on tokens and JWTs - both humans and machines can "talk" to the service.
- No shell access, no integrations, no telemetry, no "calling home".
- Simple deployment via a Docker image - with a single command, a specified directory can be exposed through a browser-based file explorer.
- The solution is implemented using mature technologies. On the backend, .NET is used, with its convenient I/O and file system access implementations supporting asynchronous operations. The web application is implemented using Angular.


## Roadmap

The current implementation is a minimal, headless proof of concept (PoC) used by third-party software solely as an API. The list below outlines the features that will be implemented if the solution "proves itself".

- Implementation of other "standard" file system functionalities that do not directly derive from the IStorageBackend abstraction.
- Full implementation of the web application, supporting authentication, token management, and all file management features.
- Directory compression and archiving.
- Support for multimedia previews (thumbnails).
- Support for rendering and previewing text files.

