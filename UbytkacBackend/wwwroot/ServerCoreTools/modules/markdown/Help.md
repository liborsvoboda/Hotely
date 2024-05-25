# Markdown Viewer HTML

## Quick Start Guide

1. Put [index.html](./dist/index.html) and [mdvh.js](./dist/mdvh.js) on your web server.
2. Create `index.md` and put it on your web server.
3. Access `index.html` with a browser.

## Markdown Format

see [markdown-it](https://github.com/markdown-it/markdown-it)

## Extended Format
### Table of Content

```
[[toc]]
```

[[toc]]

### alerts

see [markdown-it-alerts](https://github.com/nunof07/markdown-it-alerts#readme)

- example
  - success
    
    ```
    ::: success
    Hello world! [Link](#).
    :::
    ```
    
    ::: success
    Hello world! [Link](#).
    :::
    
  - info
  
    ```
    ::: info
    Hello world! [Link](#).
    :::
    ```
    
    ::: info
    Hello world! [Link](#).
    :::
    
  - warning
    
    ```
    ::: warning
    Hello world! [Link](#).
    :::
    ```
    
    ::: warning
    Hello world! [Link](#).
    :::
    
  - danger
    
    ```
    ::: danger
    Hello world! [Link](#).
    :::
    ```
    
    ::: danger
    Hello world! [Link](#).
    :::

### footnote

see [markdown-it-footnote](https://github.com/markdown-it/markdown-it-footnote)

- example

  ```
  Here is a footnote reference,[^1] and another.[^longnote]

  [^1]: Here is the footnote.
  
  [^longnote]: Here's one with multiple blocks.
  
      Subsequent paragraphs are indented to show that they
  belong to the previous footnote.
  ```
  
  Here is a footnote reference,[^1] and another.[^longnote]

  [^1]: Here is the footnote.
  
  [^longnote]: Here's one with multiple blocks.
  
      Subsequent paragraphs are indented to show that they
  belong to the previous footnote.

### mermaid

see [mermaid.js](https://mermaid-js.github.io/mermaid/#/)

- example

  ````
  ```mermaid
  graph LR
      A-->B;
      B-->C1;
      B-->C2;
      C1-->D;
      C2-->D;
  ```
  ````
  
  ```mermaid
  graph LR
      A-->B;
      B-->C1;
      B-->C2;
      C1-->D;
      C2-->D;
  ```

### math

see [markdown-it-math](https://github.com/runarberg/markdown-it-math)

- example
  
  ```
  $$$
  P(A | B) = (P(B | A)P(A)) / P(B)
  $$$
  ```
  
  $$$
  P(A | B) = (P(B | A)P(A)) / P(B)
  $$$

### video

see [markdonw-it-block-embed](https://github.com/rotorz/markdown-it-block-embed)

- example
  
  ```
  @[youtube](lJIrF4YjHfQ)
  ```

  @[youtube](lJIrF4YjHfQ)

## Dependencies

- markdown-it
- markdonw-it-anchor
- markdown-it-table-of-contents
- markdown-it-footnote
- md-it-mermaid
- mermaid
- [github-markdown-css](https://github.com/sindresorhus/github-markdown-css)
- markdown-it-alerts
- bootstrap
- markdown-it-math
- markdown-it-block-embed

## License

```
> quote 1
>> quote 2
>>> quote 3
...
```

MIT License

## link

[sub](sub)

[./sub](./sub)

[./sub/](./sub/)

[./sub/index.md](./sub/index.md)

[./sub/index.md?foo=bar&buz=qux](./sub/index.md?foo=bar&buz=qux)

[./sub/index.md#aaa](./sub/index.md#aaa)

[./sub/index.md?foo=bar&buz=qux#bbb](./sub/index.md?foo=bar&buz=qux#bbb)

[./sub/index.md#ccc](./sub/index.md#ccc)

[sub/index.md](sub/index.md)

[sub/index.md?foo=bar&buz=qux](sub/index.md?foo=bar&buz=qux)

[sub/index.md#aaa](sub/index.md#aaa)

[sub/index.md?foo=bar&buz=qux#bbb](sub/index.md?foo=bar&buz=qux#bbb)

[sub/index.md#ccc](sub/index.md#ccc)  

## highlight
<pre lang="no-highlight"><code>Inline `code` has `back-ticks around` it.
</code></pre>

        <p>Inline <code>code</code> has <code>back-ticks around</code> it.</p>

        <p>Blocks of code are either fenced by lines with three back-ticks <code>```</code>, or are indented with four spaces. I recommend only using the fenced code blocks -- they're easier and only they support syntax highlighting.</p>

<pre lang="no-highlight"><code>```javascript
var s = "JavaScript syntax highlighting";
alert(s);
```

```python
s = "Python syntax highlighting"
print s
```

```
No language indicated, so no syntax highlighting.
But let's throw in a &lt;b&gt;tag&lt;/b&gt;.
```
</code></pre>

