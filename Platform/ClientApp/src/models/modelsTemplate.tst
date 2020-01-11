${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;

  string typeInit(Property prop)
  { 
    if (prop.Type.IsPrimitive || prop.Type=="any"){
      return prop.Type;
    }
    else {
      if(prop.Type.IsEnumerable)
      {
        return prop.Type +" = []";
      }else{
        return prop.Type +" = new " + prop.Type+"()";
    }
    }
  } 

 string Imports(Class c)
    {

        //todo: add where class type is not 'object' or 'any'


        IEnumerable<Type> types = c.Properties
            .Select(p => p.Type)
            .Where(t => !t.IsPrimitive || t.IsEnum)
            .Select(t => t.IsGeneric ? t.TypeArguments.First() : t)
            .Where(t => t.Name != c.Name && t.Name != "any" && !t.Attributes.Any(attr => attr.name.ToLower().Contains("TsBypassImport")))
            .Distinct();


        var str= string.Join(Environment.NewLine, types.Select(t => $"import {{ {t.Name} }} from './{t.Name}';").Distinct());

        if(c.BaseClass !=null){
        str+= Environment.NewLine + $"import {{ {c.BaseClass} }} from './{c.BaseClass}';";
        }
        return str;
    }

  string ClassNameWithExtends(Class c) {
        return c.Name +  (c.BaseClass!=null ?  " extends " + c.BaseClass.Name : "");
    }

    // Uncomment the constructor to change template settings.
    //Template(Settings settings)
    //{
    //    settings.IncludeProject("Project.Name");
    //    settings.OutputExtension = ".tsx";
    //}

    // Custom extension methods can be used in the template by adding a $ prefix e.g. $LoudName
    string LoudName(Property property)
    {
        return property.Name.ToUpperInvariant();
    }

    
}

$Classes([Platform.Core.Attributes.TsAutoGenerateModel])[

$Imports

export class $ClassNameWithExtends {
$Properties[

     public $name: $typeInit;]
    }]
$Enums()[
export enum $Name {
    $Values[$Name = $Value][,
    ]
}]
