using System.Diagnostics.CodeAnalysis;

// this is worhtless because null coalesc uses it's own method while UnityEngine has an override to it
// which shouldn't even count as a fucking warning. grrrrr
[assembly: SuppressMessage("Style", "IDE0029:Use coalesce expression", Justification = "UnityEngine", Scope = "module")]
