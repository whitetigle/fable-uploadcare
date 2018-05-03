module UploadCare

open Elmish
open Elmish.React
open Fulma

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