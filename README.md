
# XF.ExpandableButtons

Simple xamarin button, based on TemplatedView, created to emulate a FAB.  
*No CustomRender used. To display it in top of content (including expanding) you have to put them on top of your content.*

## Package

Not available yet.  
*For a manual pack : Clone the repo, open the ExpandableButtons.sln, and Pack the ExpandableButtons project.*

## Getting started

There is two controls in this package, a `ButtonItem` that you can use to replace a simple Button. And a `PopupButton` allowing you to display a collection of `ButtonItem` (like an expander).  
All the content and visual can be personnalized by define your own ControlTemplate basing (or not) of the buid-in style.

### Setup

1. Reference the NuGet in your shared project.  
2. If you work with theme dictionnary, add the Generic theme in your merge dictionnaries.
3. Or you can call `ExpandableButtonsManager.Init()` to add the dictionnary automaticaly.

### Using a simple Button

TODO

### Using an Expandable Button

TODO

### Extend/Modify default style

TODO

### Try it

This repo contain a testable Android/iOS sample, you can launch it from the `ExpandableButtons.sln` solution.