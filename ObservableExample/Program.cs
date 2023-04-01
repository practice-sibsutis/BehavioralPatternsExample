// See https://aka.ms/new-console-template for more information
using System.Reactive.Linq;
using System.Reactive.Subjects;

Subject<int> subject = new Subject<int>();
subject.Where(s => s < 5)
       .Subscribe(Console.WriteLine);

subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(3);
subject.OnNext(4);

subject.OnNext(5);
subject.OnNext(6);
subject.OnNext(7);
subject.OnNext(8);

subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(3);
subject.OnNext(4);
