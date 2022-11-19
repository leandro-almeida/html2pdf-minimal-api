# Html2Pdf Minimal Api - .NET 7

Hey!
This is a simple HTTP API for converting HTML string content into PDF binary.

## Request 

There is a single endpoint: `POST /convert`, receiving the following json body scheme:
```json
{
  "htmlContentBase64": "string",
  "docTitle": "string",
  "orientation": 0,
  "customMargins": {
    "unit": 0,
    "top": 0,
    "right": 0,
    "bottom": 0,
    "left": 0
  },
  "customFooter": {
    "fontSize": 0,
    "fontName": "string",
    "textLeft": "string",
    "textRight": "string",
    "textCenter": "string",
    "showLineAbove": true
  }
}
```

The following objects are optional:
- docTitle: you can specify a document title for the resulting PDF file
- orientation: [0] Landscape, [1] Portrait (default)
- customMargins:
  - unit: Unit of Measurement - [0] Inches, [1] Millimeters, [2] Centimeters
- customFooter: you can customize which text appears on footer in all pages.

## Response
The PDF binary (bytes) file is included in the body section of the response as Base64 encoded string. 

