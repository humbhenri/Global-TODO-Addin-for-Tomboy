all: GlobalTodo.cs GlobalTodo.addin.xml
	gmcs -debug -out:GlobalTodo.dll -target:library -pkg:tomboy-addins -pkg:gtk-sharp-2.0  -r:Mono.Posix GlobalTodo.cs -resource:GlobalTodo.addin.xml
