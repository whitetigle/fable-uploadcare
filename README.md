# Fable UploadCare Widget

Try it: click [here](https://whitetigle.github.io/fable-uploadcare/)

## Building and running the app

> In the commands below, yarn is the tool of choice. If you want to use npm, just replace `yarn` by `npm` in the commands.

* `cd UploadCare`
* Install dependencies: `yarn`
* Start Fable daemon and [Webpack](https://webpack.js.org/) dev server: `yarn start`
* In your browser, open: http://localhost:8084/

## How to use in your project
* Add dependency: `yarn add uploadcare-widget`
* Grab the `src/UploadCarecomponent.fs` file and add it to your project.


## How does it work?

### Prepare a component helper
 ```f#
  let inline UploadCare props = ofType<UploadCareComponent.Input,_,_> props []
```

### Use it in your React view
```f#
 UploadCare  
    { 
        value=""
        onChange=(fun _ -> printfn "onChange")
        onUploadComplete=(fun info -> printfn "%s" info.cdnUrl)
    }
```

### Update your index.html with credentials

```html
<script>
  UPLOADCARE_PUBLIC_KEY = 'demopublickey'; // change to your private key
</script>
```

### Use it with Elmish

Just have a look at `src/App.fs` to see a complete sample

## Disclaimer
> The whole UploadCare API has not been handled in this project. It's merely a simple sample you can then change according to your needs.

> So fork this project or send a PR if you wish to add more bindings.

## Credits
Thanks to @vbfox for his great [React to fable tutorials](https://blog.vbfox.net/2018/02/06/fable-react-1-react-in-fable-land.html)!
