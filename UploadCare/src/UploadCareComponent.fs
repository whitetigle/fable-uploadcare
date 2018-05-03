module UploadCareComponent
    
open Fable.Core
open Fable.Import
open Fable.Core.JsInterop
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
