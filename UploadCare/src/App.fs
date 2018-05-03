module UploadCare

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Elmish
open Elmish.React
open Fulma

module UploadCareComponent = 
    
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    
    [<Pojo>]
    type FileInfo = 
        {
            uuid: string
            name: string
            size:float
            isStored:bool
            isImage:bool
            cdnUrl:string
            cdnUrlModifiers:string
            originalUrl:string
            originalImageInfo:obj
            sourceInfo:obj
        }

    [<Pojo>]
    type Props = 
        { 
            value: string
            ; onChange: obj->unit
            ; onUploadComplete: FileInfo->unit
        }

    type IUploadCareWidget = 
        abstract value : string  -> unit with get, set 
        abstract onChange : (obj  -> unit) -> unit  with get, set 
        abstract onUploadComplete : (FileInfo  -> unit) -> unit with get, set 

    type IUploadCareStatic = 
        abstract Widget : Browser.HTMLInputElement option -> IUploadCareWidget with get, set 
        
    let UploadCare : IUploadCareStatic = importDefault "uploadcare-widget"
            

    type Input(props) =
        inherit React.Component<Props, obj>(props)

        let mutable inputField: Browser.HTMLInputElement option = None

        override this.componentDidMount () =
            let widget = UploadCare.Widget(inputField)
            widget.value props.value
            widget.onChange props.onChange
            widget.onUploadComplete props.onUploadComplete
            
        override this.render () =        
            input [
                Ref (fun x -> inputField <- Some(x:?>Browser.HTMLInputElement))
                Type "hidden"
                Role "uploadcare-uploader"
                Name "content"
            ]        

module Types = 
    type Model = 
        {
            CdnURL:string option
        }

    type Msg = 
        | DisplayCDNURl of string
        | TryAgain

module View = 

    open Types
    open Fable.Helpers.React
    open Fable.Helpers.React.Props

    let root (model:Model) dispatch = 
        
        let ui = 
            let inline UploadCare props = ofType<UploadCareComponent.Input,_,_> props []
            match model.CdnURL with 
            | Some url -> 
                let message = sprintf "Link: %s" url
                [
                    Heading.h3[][ str message]
                    Button.button[ Button.Props[OnClick (fun e -> dispatch TryAgain )]][str "Try again"]
                ]
            | None -> 
                [
                    Heading.h1[][ str "Fable: UploadCare Widget Test"]
                    UploadCare  
                        { 
                            value=""
                            onChange=(fun _ -> printfn "onChange")
                            onUploadComplete=(fun info -> (DisplayCDNURl info.cdnUrl) |> dispatch)
                        }
                ]  
        
        Hero.hero [ Hero.Color IsDark; Hero.IsFullHeight  ] [
            Hero.body [] [
                Container.container[] ui                
            ]
        ]


module State = 
    open Types 

    let init _ = {CdnURL=None}, Cmd.none

    let update (msg: Msg) (model: Model) = 
        match msg with 
        | DisplayCDNURl url ->  
            {model with CdnURL = Some url}, Cmd.none
        | TryAgain -> 
            {model with CdnURL = None}, Cmd.none

let init() =
    Program.mkProgram State.init State.update View.root
    |> Program.withReact"elmish-app"
    |> Program.run

init()