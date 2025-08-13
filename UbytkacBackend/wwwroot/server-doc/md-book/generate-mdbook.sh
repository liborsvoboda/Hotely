#!/bin/bash

# Optional Download & Build example
# mkdir bin
# curl -sSL https://github.com/rust-lang/mdBook/releases/download/v0.4.35/mdbook-v0.4.35-x86_64-unknown-linux-gnu.tar.gz | tar -xz --directory=bin
# bin/mdbook build

SCRIPT=$(realpath -s "$0")
SCRIPTPATH=$(dirname "$SCRIPT")

cd "${SCRIPTPATH}"
chmod u+r+x "${SCRIPTPATH}"/mdbook

"${SCRIPTPATH}"/mdbook clean
"${SCRIPTPATH}"/mdbook init --theme "rust" --force --ignore=none
"${SCRIPTPATH}"/mdbook build