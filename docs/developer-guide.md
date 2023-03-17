# Developer Guide

The following document describes the setup and workflow that is recommended for working on the Windows Forms project. It assumes that you have read the [Contributing Guide](../CONTRIBUTING.md).

The [Issue Guide](issue-guide.md) describes our approach to using GitHub issues.

## Machine Setup

### Windows

TBD

### Ubuntu


#### Pre-requisite

Install WSL, refer to https://learn.microsoft.com/windows/wsl/install.

#### Configure Ubuntu in WSL environment

1. Install Ubuntu in WSL and make it the default distro:

    ```powershell
    # PowerShell
    wsl --install
    wslconfig /setdefault Ubuntu
    ```

1. Configure [sharing git credentials between Windows and WSL](https://code.visualstudio.com/docs/remote/troubleshooting#_sharing-git-credentials-between-windows-and-wsl):

    ```bash
    # bash
    git config --global credential.helper "/mnt/c/Program\ Files/Git/mingw64/bin/git-credential-manager-core.exe"
    ```

    Now you should be able to clone a repo.

1. Install PowerShell

    ```bash
    # bash
    sudo apt-get update && \
        sudo apt-get install powershell
    ```

1. Install .NET SDKs - .NET Core 3.1/6/7

    Follow the instructions in [Install .NET SDK or .NET Runtime on Ubuntu 22.04](https://learn.microsoft.com/dotnet/core/install/linux-ubuntu-2204#install-net-7).

    ```bash
    # bash
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb

    sudo apt-get update && \
        sudo apt-get install -y dotnet-sdk-3.1 \
        sudo apt-get install -y dotnet-sdk-6.0 \
        sudo apt-get install -y dotnet-sdk-7.0
    ```

1. Install .NET 8 SDK

    ```bash
    # bash
    wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
    sudo chmod +x ./dotnet-install.sh
    sudo ./dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet
    ```

1. Install OpenSSL 1.1

    ```bash
    # bash
    wget http://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.1f-1ubuntu2.17_amd64.deb
    sudo dpkg -i libssl1.1_1.1.1f-1ubuntu2.17_amd64.deb
    rm libssl1.1_1.1.1f-1ubuntu2.17_amd64.deb
    ```

    > NOTE: if you get HTTP 404, check what the latest version is available at http://archive.ubuntu.com/ubuntu/pool/main/o/openssl/ and adjust the command accordingly.

1. Configure git:

    ```bash
    # bash
    git config --global user.email "your.name@email.com"
    git config --global user.name "Your Name"
    git config --global core.editor "code --wait"
    ```

1. To open the project in VS Code run:

    ```bash
    # bash
    code .
    ```

1. Now you are all set to build and test from Ubuntu!

#### Configure Visual Studio to run tests in Ubuntu WSL

Create the SDK solution as you normally would and open it in Visual Studio. To enable test execution in Ubuntu WSL follow https://learn.microsoft.com/visualstudio/test/remote-testing.

![Test Explorer](./images/linux-test-explorer.png)

When you run a test in WSL for the first time, you will likely get prompted to install the required debugging tools. Just follow the prompt:

    > This remote runtime environment is missing prerequisites (.NET/vsdbg or related tooling) required to discover/run/debug tests.
    > Attempting to install them automatically. More details can be found at https://aka.ms/remotetesting
    >
    > [sudo] password for user:
    >
    > : Information: Install Unit Testing Prerequisites script
    > : Information: Visual Studio debugger bits not installed
    > : Information: Installing VS debugger bits to /usr/local/.vsdbg/
    > Info: Previous installation at '/usr/local/.vsdbg/' not found
    > Info: Using vsdbg version '17.6.10208.1'
    > Using arguments
    >     Version                    : 'latest'
    >     Location                   : '/usr/local/.vsdbg'
    >     SkipDownloads              : 'false'
    >     LaunchVsDbgAfter           : 'false'
    >     RemoveExistingOnUpgrade    : 'false'
    > Info: Using Runtime ID 'linux-x64'
    > Downloading https://vsdebugger.azureedge.net/vsdbg-17-6-10208-1/vsdbg-linux-x64.tar.gz

Now, if you put a breakpoint in a test, you should see something similar to the following:

![Test Explorer](./images/linux-test-env.png)

#### Manage repos located in WSL in Windows

You can continue to manage repos located in WSL in Windows with [Git Extensions](https://github.com/gitextensions/gitextensions).

1. You can open the repo from a path starting with `\\wsl$\Ubuntu`, e.g.: `\\wsl$\Ubuntu\home\<user>\dev\r9-samples` (assuming you cloned the repo under `~/dev/r9-samples).
1. Before you can commit any changes you may need to configure some of the git settings.

The "editor" is run in WSL, so it should be accessible/executable in it - hence, "`code .`". The diff/merge tools are executed in Windows, so these should be mapped to your local Windows environment.

    ![Test Explorer](./images/linux-gitextensions.png)
